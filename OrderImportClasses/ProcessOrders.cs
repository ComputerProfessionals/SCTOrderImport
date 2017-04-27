using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderImportClasses.SCTWS;
using System.ServiceModel;
using System.Configuration;
using System.Reflection;
using System.Net;

namespace OrderImportClasses
{
    public class ProcessOrders
    {

        string GetString(Object value)
        {
            if (value != null && value != DBNull.Value)
                return value.ToString();
            else
                return "";
        }

        decimal GetDecimal(Object value)
        {
            if (value != null && value != DBNull.Value)
                return (decimal)(value);
            else
                return 0;
        }

        private string GetTimeCode(string psTime)
        {
            string lsCode = "";
            using (SCTModel db = new SCTModel())
            {
                TimeCodeMap tcm = db.TimeCodeMaps.Where(p => p.TimeVal == psTime).FirstOrDefault();

                if (tcm != null)
                    lsCode = tcm.TimeCode.GetValueOrDefault(0).ToString();

            }

            return lsCode; 
            
        }

        public string Process()
        {
            string lsResult = "true";
            int liDone = 0;
            SCTWS.zsd_create_web_so ws=null;
            try

            {               

                string strUserName = GetString(ConfigurationManager.AppSettings["SAPUserName"]);
                string strPassword = GetString(ConfigurationManager.AppSettings["SAPUserPassword"]);

                if (strUserName == "")
                    strUserName = "TEST_SL.PRTL";

                if (strPassword == "")
                    strPassword = "logis123";

                if (strUserName == "" || strPassword == "")
                {
                    Log.LogError("Failed to get sap username and or password", "OrderImportClasses.ProcessOrders");
                    return "Failed to get sap username and or password";                   
                }

                List<ShippingRequestHeader> headerList;
                
                using (SCTModel hdb = new SCTModel())
                {
                    headerList = (from p in hdb.ShippingRequestHeaders where p.ProcessedDate == null select p).ToList();
                }

                foreach (ShippingRequestHeader h in headerList)
                {

                    List<ShippingRequestDetail> itemList = null;

                    using (SCTModel hdb = new SCTModel())
                    {
                        itemList = (from p in hdb.ShippingRequestDetails where p.ShipReqID == h.ShipReqID select p).ToList();
                    }

                    if (itemList != null && itemList.Count > 0)
                    {

                       // Log.LogError("Doing header " + h.ShipReqID.ToString(), "OrderImportClasses.ProcessOrders");

                        liDone += 1;

                        string lsMessage = "";
                        string lsSAPOrderNumber = "";

                        ZSD_CREATE_WEB_SALES_ORDER so = new ZSD_CREATE_WEB_SALES_ORDER();

                        so.IS_SO_HEADER = new ZSD_WEB_SO_HEADER_S();

                        ZSD_WEB_SO_HEADER_S header = so.IS_SO_HEADER;
                        header.BSTKD = GetString(h.CustomerPONum);
                        header.BSTKD_E = GetString(h.ConsignmentRef);
                        header.TEMP = GetString(h.TemperatureID);
                        header.TRATY = GetString(h.MeansOfTransportID);
                        header.KUNAG = GetString(h.CustomerAccountID);
                        header.SHIPFR = GetString(h.PickupFromID);
                        header.BSTDK = GetString(h.PickupDate.ToString("yyyy-MM-dd"));
                        header.PICKUP = GetTimeCode(GetString(h.PickupTime));

                        if (header.PICKUP == "")
                        {
                            lsResult = "Unable to resolve PICKUP time code for " + GetString(h.PickupTime);
                            Log.LogError(lsResult, "OrderImportClasses.ProcessOrders");
                            return lsResult;
                        }
                        header.SHIPTO = GetString(h.DeliverToID);
                        header.VDATU = GetString(h.DeliveryDate.ToString("yyyy-MM-dd"));
                        header.DELCO = GetTimeCode(GetString(h.DeliveryTime));
                        if (header.DELCO == "")
                        {
                            lsResult = "Unable to resolve DELCO time code for " + GetString(h.PickupTime);
                            Log.LogError(lsResult, "OrderImportClasses.ProcessOrders");
                            return lsResult;
                        }
                        header.DELIV_INSTR = GetString(h.DeliveryInstructions);

                        List<ZSD_WEB_SO_ITEM_S> items = new List<ZSD_WEB_SO_ITEM_S>();

                        foreach (ShippingRequestDetail i in itemList)
                        {
                            ZSD_WEB_SO_ITEM_S item = new ZSD_WEB_SO_ITEM_S();
                            item.BRGEW = GetDecimal(i.GrossWeight);
                            item.KWMENG = GetDecimal(i.Quantity);
                            item.MATNR = GetString(i.MaterialNumber);
                            item.MEINS = GetString(i.UnitOfMeasure);
                            item.NTGEW = GetDecimal(i.NetWeight);
                            item.VOLUM = GetDecimal(i.Volume);
                            items.Add(item);
                        }

                        so.IT_SO_ITEM = items.ToArray();

                        List<BAPIRET2> retlist = new List<BAPIRET2>();

                        BAPIRET2 bret = new BAPIRET2();

                        retlist.Add(bret);
                        so.ET_RETURN = retlist.ToArray();                        

                        ZSD_CREATE_WEB_SALES_ORDERResponse ret = null;

                        bool lbOK = false;

                        if (ws == null)
                        {
                            ws = new SCTWS.zsd_create_web_so();
                            ws.Timeout = 60000;                            
                            ws.Url =  ws.Url + "?ts=" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
                            NetworkCredential netCredential = new NetworkCredential(strUserName, strPassword);
                            Uri uri = new Uri(ws.Url);
                            ICredentials credentials = netCredential.GetCredential(uri, "Basic");
                            ws.Credentials = credentials;
                        }

                        try
                        {
                          //  Log.LogError("Pre send web request", "OrderImportClasses.ProcessOrders");
                            ret = ws.ZSD_CREATE_WEB_SALES_ORDER(so);
                          //  Log.LogError("Post send web request", "OrderImportClasses.ProcessOrders");
                            if (ret != null && ret.ET_RETURN != null && ret.ET_RETURN.Length > 0)
                            {

                                lbOK = true;

                                for (int i = 0; i < ret.ET_RETURN.Length; i++)
                                {
                                    BAPIRET2 returnLine = ret.ET_RETURN[i];
                                    string allMessages = returnLine.MESSAGE + " " +
                                            returnLine.MESSAGE_V1 + " " +
                                            returnLine.MESSAGE_V2 + " " +
                                            returnLine.MESSAGE_V3 + " " +
                                            returnLine.MESSAGE_V4;

                                    if (returnLine.TYPE.ToUpper() != "S")
                                    {
                                        lbOK = false;
                                        lsMessage += " " + allMessages;                                       
                                    }
                                    else
                                    {
                                        if (returnLine.NUMBER == "311")
                                        {
                                            allMessages = allMessages.ToLower();
                                            int start = allMessages.IndexOf("booking request");
                                            allMessages = allMessages.Substring(start + "booking request".Length + 1);
                                            start = allMessages.IndexOf("has been saved");
                                            lsSAPOrderNumber = allMessages.Substring(0, start).Trim();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Log.LogError("Failed to import order", "OrderImportClasses.ProcessOrders");
                                lsMessage = "Failed to import order id=" + h.ShipReqID.ToString();
                            }

                        }
                        catch (Exception ex)
                        {

                            Log.LogError(ex.ToString(), "OrderImportClasses.ProcessOrders");
                            lsMessage = ex.ToString();
                        }

                        using (SCTModel headerDB = new SCTModel())
                        {
                            ShippingRequestHeader loHeader = headerDB.ShippingRequestHeaders.Find(h.ShipReqID);

                            if (lbOK)
                            {
                                loHeader.ProcessedDate = DateTime.Now;
                                loHeader.SAPOrderNumber = lsSAPOrderNumber;
                                lsMessage += " created order " + lsSAPOrderNumber;
                            }
                            else {
                                loHeader.ProcessedDate = null;
                                loHeader.SAPOrderNumber = "";
                                Log.LogError("Sales Request Header " + h.ShipReqID.ToString() + " failed: " +  lsMessage, "OrderImportClasses.ProcessOrders");
                            }

                            loHeader.ProcessedSuccessfully = lbOK;
                            loHeader.UpdateDate = System.DateTime.Now;
                            headerDB.SaveChanges();
                        }

                        if (lsMessage != "")
                        {                           

                            if (lsResult != "true")
                                lsResult = lsMessage;
                            else
                            {
                                lsResult += System.Environment.NewLine + lsMessage;
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                Log.LogError(ex.ToString(), "OrderImportClasses.ProcessOrders");
                lsResult = ex.ToString();
            }
            finally
            {
                if (ws != null)
                {
                    ws.Dispose();                   
                }
            }

            return lsResult + "  - did " + liDone.ToString() + " records";
        }

        public void EncryptConfigSection(string psKey)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection section = config.GetSection(psKey);

            if (section != null)
            {
                if (!section.SectionInformation.IsProtected)
                {
                    if (!section.ElementInformation.IsLocked)
                    {
                        section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                        // section.SectionInformation.ForceSave = true;
                        config.Save();
                    }
                }
            }
        }

        public void DecryptConfigSection(string psKey)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection section = config.GetSection(psKey);

            if (section != null)
            {
                if (section.SectionInformation.IsProtected)
                {
                    if (!section.ElementInformation.IsLocked)
                    {
                        section.SectionInformation.UnprotectSection();
                        // section.SectionInformation.ForceSave = true;
                        config.Save();
                    }
                }
            }
        }

    }
}