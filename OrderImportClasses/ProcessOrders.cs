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
        public void Process()
        {
            SRT_SCT db = new SRT_SCT();

            try

            {

                foreach (ShippingRequestHeader h in (from p in db.ShippingRequestHeaders where p.ProcessedDate == null select p))
                {

                  //  ZSD_CREATE_WEB_SO SO;// = new SCTWS.ZSD_CREATE_WEB_SO();
                  //  ZSD_CREATE_WEB_SALES_ORDERRequest req = new SCTWS.ZSD_CREATE_WEB_SALES_ORDERRequest();

                   // req.ZSD_CREATE_WEB_SALES_ORDER = new ZSD_CREATE_WEB_SALES_ORDER();
                    ZSD_CREATE_WEB_SALES_ORDER so = new ZSD_CREATE_WEB_SALES_ORDER();// req.ZSD_CREATE_WEB_SALES_ORDER;

                   ZSD_WEB_SO_HEADER_S header = so.IS_SO_HEADER;
                    header.BSTKD = h.CustomerPONum;
                    header.BSTKD_E = h.ConsignmentRef;
                    header.TEMP = h.TemperatureID;
                    header.TRATY = h.MeansOfTransportID;
                    header.KUNAG = h.CustomerAccountID;
                    header.SHIPFR = h.PickupFromID;
                    header.BSTDK = h.PickupDate.ToString("yyyy/mm/dd");
                    header.PICKUP = h.PickupTime;
                    header.SHIPTO = h.DeliverToID;
                    header.VDATU = h.DeliveryDate.ToString("yyyy/mm/dd");
                    header.DELCO = h.DeliveryTime;
                    header.DELIV_INSTR = h.DeliveryInstructions;

                    List<ZSD_WEB_SO_ITEM_S> items = new List<ZSD_WEB_SO_ITEM_S>();

                    foreach (ShippingRequestDetail i in (from d in db.ShippingRequestDetails where d.ShipReqID == h.ShipReqID select d))
                    {
                        ZSD_WEB_SO_ITEM_S item = new ZSD_WEB_SO_ITEM_S();
                        item.BRGEW = i.GrossWeight;
                        item.KWMENG = i.Quantity;
                        item.MATNR = i.MaterialNumber;
                        item.MEINS = i.UnitOfMeasure;
                        item.NTGEW = i.NetWeight;
                        item.VOLUM = i.Volume;
                        items.Add(item);                                              
                    }

                    so.IT_SO_ITEM = items.ToArray();                   

                    BasicHttpBinding httpBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);                                      

                    httpBinding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Basic;

                    string wsurl = ConfigurationManager.AppSettings["SAPWSURL"];

                    EndpointAddress endPointAddr = new EndpointAddress(wsurl);

                    ZSD_CREATE_WEB_SOClient ws = new ZSD_CREATE_WEB_SOClient(httpBinding, endPointAddr);

                    string strUserName =  ConfigurationManager.AppSettings["SAPUserName"];
                    string strPassword = "logis123";

                  //  System.Net.NetworkCredential cred = new System.Net.NetworkCredential(strUserName, strPassword);                   

                  //  cred.Domain = "";

                    ws.ClientCredentials.UserName.UserName = strUserName;
                    ws.ClientCredentials.UserName.Password = strPassword;

                    //  ws.ChannelFactory.Credentials.Windows.ClientCredential = cred;
                    // ws.ClientCredentials.Windows.AllowNtlm = false;
                    // service.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.   
                                             
                    ZSD_CREATE_WEB_SALES_ORDERResponse ret = null;

                    bool lbOK = false;                   

                    try
                    {
                        ret = ws.ZSD_CREATE_WEB_SALES_ORDER(so);
                        if (ret != null && ret.ET_RETURN.Length > 2)
                        {
                            if (ret.ET_RETURN[2].TYPE.Equals("S", StringComparison.OrdinalIgnoreCase))
                            {
                                lbOK = true;
                            }
                            else 
                            {
                                string lsMessaage = ret.ET_RETURN[3].MESSAGE + " " + 
                                    ret.ET_RETURN[3].MESSAGE_V1 +  " " + 
                                    ret.ET_RETURN[3].MESSAGE_V2 + " " + 
                                    ret.ET_RETURN[3].MESSAGE_V3 + " " + 
                                    ret.ET_RETURN[3].MESSAGE_V4;

                                Log.LogError("Failed to process order: " + lsMessaage.Trim(), "OrderImportClasses.ProcessOrders");
                            }
                            
                        }

                    } catch(Exception ex){
                        
                        Log.LogError(ex.ToString(), "OrderImportClasses.ProcessOrders");
                    }

                    h.ProcessedDate =  DateTime.Now;
                    h.ProcessedSuccessfully = lbOK;
                    h.UpdateDate = System.DateTime.Now;
                    
                    db.SaveChanges();
                   
                }

            }

            catch (Exception ex)
            {
                Log.LogError(ex.ToString(), "OrderImportClasses.ProcessOrders");
            }
        }
    }
}
