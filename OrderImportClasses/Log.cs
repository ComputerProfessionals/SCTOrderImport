using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderImportClasses
{
    public static class Log
    {
        public static void LogError(string psMessage, string psModule)
        {
            ShippingRequestError E = new ShippingRequestError();// new ShippingRequestError();
            E.DateCreated = System.DateTime.Now;
            E.Module = psModule;
            E.ErrorDetail = psMessage;
            using  (SCTModel db = new SCTModel()){
                db.ShippingRequestErrors.Add(E);
                db.SaveChanges();
            }           
        }
    }
}
