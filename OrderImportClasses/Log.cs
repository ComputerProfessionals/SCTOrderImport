using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace OrderImportClasses
{
    public static class Log
    {
        public static void LogError(string psMessage, string psModule, Int32 piShipReqID = 0)
        {
            ShippingRequestError E = new ShippingRequestError();// new ShippingRequestError();
            E.DateCreated = System.DateTime.Now;
            E.Module = psModule;
            E.ErrorDetail = psMessage;
            E.ShipReqID = piShipReqID;
            using  (SCT db = new SCT()){
                db.ShippingRequestErrors.Add(E);
                db.SaveChanges();
            }           
        }
    }
}
