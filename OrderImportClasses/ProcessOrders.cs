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
using DataModel;

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
            using (SCT db = new SCT())
            {
                TimeCodeMap tcm = db.TimeCodeMaps.Where(p => p.TimeVal == psTime).FirstOrDefault();

                if (tcm != null)
                {
                    lsCode = tcm.TimeCode.GetValueOrDefault(0).ToString();
                    if (lsCode.Length < 2)
                        lsCode = "0" + lsCode;
                }

            }

            return lsCode; 
            
        }

        public bool Process(out string psMessages)
        {
            bool lbResult = true;
            psMessages = "";           
           

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
                    psMessages= "Failed to get sap username and or password";
                    return false;                   
                }

                List<ShippingRequestHeader> headerList;
                
                using (SCT hdb = new SCT())
                {
                    headerList = (from p in hdb.ShippingRequestHeaders where p.ProcessedDate == null select p).ToList();
                }

                if (headerList.Count > 0)
                {
                    SCTWS.zsd_create_web_so ws = new SCTWS.zsd_create_web_so();
                    ws.Timeout = 60000;
                    ws.Url = ws.Url + "?ts=" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
                    NetworkCredential netCredential = new NetworkCredential(strUserName, strPassword);
                    Uri uri = new Uri(ws.Url);
                    ICredentials credentials = netCredential.GetCredential(uri, "Basic");
                    ws.Credentials = credentials;                

                    foreach (ShippingRequestHeader h in headerList)
                    {
                       string lsMessage = "";
                       if (!ProcessHeader(ws, h.ShipReqID, out lsMessage))
                        {
                            psMessages += System.Environment.NewLine + lsMessage;
                            lbResult = false;
                        }
                    }
                }
                else
                {
                    psMessages = "No unprocessed headers found.";
                }              
              
            }

            catch (Exception ex)
            {
                Log.LogError(ex.ToString(), "OrderImportClasses.ProcessOrders");
                psMessages = ex.ToString();
                lbResult = false;
            }           

            return lbResult;
        }

        private bool ProcessHeader(SCTWS.zsd_create_web_so poWS, Int32  piShipReqID, out string psMessage)
        {
            bool lbResult = true;

            psMessage = "";

            string lsMessage = "";

            SCT db = new SCT();

            try {               

                List<ShippingRequestDetail> itemList = null;
                ShippingRequestHeader h = db.ShippingRequestHeaders.Find(piShipReqID);
                h.ProcessedDate = System.DateTime.Now;
                db.SaveChanges(); 
              
                itemList = (from p in db.ShippingRequestDetails where p.ShipReqID == piShipReqID select p).ToList();                

                if (itemList != null && itemList.Count > 0)
                {
                   
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
                        lsMessage = "Unable to resolve PICKUP time code for " + GetString(h.PickupTime);
                        Log.LogError(lsMessage, "OrderImportClasses.ProcessOrder", piShipReqID);
                        return false;
                    }

                    header.SHIPTO = GetString(h.DeliverToID);
                    header.VDATU = GetString(h.DeliveryDate.ToString("yyyy-MM-dd"));
                    header.DELCO = GetTimeCode(GetString(h.DeliveryTime));

                    if (header.DELCO == "")
                    {
                        psMessage = "Unable to resolve DELCO time code for " + GetString(h.PickupTime);
                        Log.LogError(psMessage, "OrderImportClasses.ProcessOrder", piShipReqID);
                        return false;
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

                    try
                    {
                        // Log.LogError("Pre send web request", "OrderImportClasses.ProcessOrders");
                        ret = poWS.ZSD_CREATE_WEB_SALES_ORDER(so);
                        // Log.LogError("Post send web request", "OrderImportClasses.ProcessOrders");
                        if (ret != null && ret.ET_RETURN != null && ret.ET_RETURN.Length > 0)
                        {

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
                                    lbResult = false;
                                    psMessage += " " + allMessages;
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
                            psMessage += "No return data from web service.";
                            lbResult = false;
                        }

                    }
                    catch (Exception ex)
                    {

                        psMessage = ex.ToString();
                        lbResult = false;
                    }                  
                       

                    if (lbResult)
                    {
                        h.SAPOrderNumber = lsSAPOrderNumber;
                        psMessage += " created order " + lsSAPOrderNumber;                      
                    }
                    else {
                        h.SAPOrderNumber = "";

                        Log.LogError("Failed to create Sales Request Header. " + psMessage + " Customer ID = " + so.IS_SO_HEADER.KUNAG + "; Order No = " + so.IS_SO_HEADER.BSTDK,
                            "OrderImportClasses.ProcessOrder", h.ShipReqID);                        
                    }

                    h.ProcessedSuccessfully = lbResult;
                    h.UpdateDate = System.DateTime.Now;
                    db.SaveChanges();
                    
                }
            }
            catch(Exception e)
            {
                Log.LogError(e.ToString(), "ProcessOrders.ProcessOrder", piShipReqID);
            }
            finally
            {
                if (db != null)
                    db.Dispose();
            }

            return lbResult;

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