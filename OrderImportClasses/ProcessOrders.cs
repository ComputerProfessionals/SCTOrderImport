using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderImportClasses.SCTWS;
using System.ServiceModel;
using System.Configuration;
using System.Reflection;

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
        public string Process()
        {
            string lsResult = "true";
            int liDone = 0;      

            try

            {
                List<ShippingRequestHeader> headerList;
                
                using (SCTModel hdb = new SCTModel())
                {
                    headerList = (from p in hdb.ShippingRequestHeaders where p.ProcessedDate == null select p).ToList();
                }

                foreach (ShippingRequestHeader h in headerList)
                {
                    liDone += 1;

                    string lsMessage = "";                  

                    ZSD_CREATE_WEB_SALES_ORDER so = new ZSD_CREATE_WEB_SALES_ORDER();// req.ZSD_CREATE_WEB_SALES_ORDER;
                    so.IS_SO_HEADER = new ZSD_WEB_SO_HEADER_S();
                   ZSD_WEB_SO_HEADER_S header = so.IS_SO_HEADER;
                    header.BSTKD = GetString(h.CustomerPONum) ;
                    header.BSTKD_E = GetString(h.ConsignmentRef);
                    header.TEMP = GetString(h.TemperatureID);
                    header.TRATY = GetString(h.MeansOfTransportID);
                    header.KUNAG = GetString(h.CustomerAccountID);
                    header.SHIPFR = GetString(h.PickupFromID);
                    header.BSTDK = GetString(h.PickupDate.ToString("yyyyMMdd"));
                    header.PICKUP = GetString(h.PickupTime);
                    header.SHIPTO = GetString(h.DeliverToID);
                    header.VDATU = GetString(h.DeliveryDate.ToString("yyyyMMdd"));
                    header.DELCO = GetString(h.DeliveryTime);
                    header.DELIV_INSTR = GetString(h.DeliveryInstructions);

                    List<ZSD_WEB_SO_ITEM_S> items = new List<ZSD_WEB_SO_ITEM_S>();

                    List<ShippingRequestDetail> itemList;

                    using (SCTModel hdb = new SCTModel())
                    {
                        itemList = (from p in hdb.ShippingRequestDetails where p.ShipReqID == h.ShipReqID select p).ToList();
                    }

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

                  //  BasicHttpBinding httpBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);                                      

                   // httpBinding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Basic;

                   // string wsurl = ConfigurationManager.AppSettings["SAPWSURL"];

                   // EndpointAddress endPointAddr = new EndpointAddress(wsurl);                                       

                  // SCTWS.ZSD_CREATE_WEB_SALES_ORDER  ws = new ZSD_CREATE_WEB_SALES_ORDER();

                    string strUserName =  ConfigurationManager.AppSettings["SAPUserName"];
                    string strPassword = ConfigurationManager.AppSettings["SAPUserPassword"];

                    SCTWS.zsd_create_web_so ws = new SCTWS.zsd_create_web_so();
                    System.Net.ICredentials cred = new System.Net.NetworkCredential(strUserName, strPassword);
                    ws.Credentials = cred; 

                    ZSD_CREATE_WEB_SALES_ORDERResponse ret = null;

                    bool lbOK = false;                                       

                    try
                    {
                        ret = ws.ZSD_CREATE_WEB_SALES_ORDER(so);

                        if (ret != null && ret.ET_RETURN != null && ret.ET_RETURN.Length > 0)
                        {

                            lbOK = true;                           
                            
                            for (int i = 0; i < ret.ET_RETURN.Length; i++)
                            {
                                BAPIRET2 returnLine = ret.ET_RETURN[i];
                                if (returnLine.TYPE.ToUpper() != "S")
                                {
                                    lbOK = false;
                                    lsMessage += " " + returnLine.MESSAGE + " " +
                                        returnLine.MESSAGE_V1 + " " +
                                        returnLine.MESSAGE_V2 + " " +
                                        returnLine.MESSAGE_V3 + " " +
                                        returnLine.MESSAGE_V4;
                                }
                            }                          

                        }
                        else
                        {
                            Log.LogError("Failed to import order", "OrderImportClasses.ProcessOrders");
                            lsMessage = "Failed to import order id=" + h.ShipReqID.ToString();
                        }                       

                    } catch(Exception ex){
                        
                        Log.LogError(ex.ToString(), "OrderImportClasses.ProcessOrders");
                        lsMessage = ex.ToString();
                    }

                    using (SCTModel headerDB = new SCTModel())
                    {
                        ShippingRequestHeader loHeader = headerDB.ShippingRequestHeaders.Find(h.ShipReqID);
                        loHeader.ProcessedDate =  DateTime.Now;
                        loHeader.ProcessedSuccessfully = lbOK;
                        loHeader.UpdateDate = System.DateTime.Now;
                        headerDB.SaveChanges();
                    }      
                    
                    if (lsMessage != "")
                    {
                        if (lsResult == "true")
                            lsResult = lsMessage;
                        else
                        {
                            lsResult += System.Environment.NewLine + lsMessage;
                        }
                    } 
                }
            }

            catch (Exception ex)
            {
                Log.LogError(ex.ToString(), "OrderImportClasses.ProcessOrders");
                lsResult = ex.ToString();
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