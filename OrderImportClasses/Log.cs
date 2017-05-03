using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using System.Diagnostics;

namespace OrderImportClasses
{
    public static class Log
    {
        public static void LogError(string psMessage, string psModule, Int32 piShipReqID = 0)
        {
            try {
                ShippingRequestError E = new ShippingRequestError();// new ShippingRequestError();
                E.DateCreated = System.DateTime.Now;
                E.Module = psModule;
                E.ErrorDetail = psMessage;
                E.ShipReqID = piShipReqID;
                using (SCT db = new SCT()) {
                    db.ShippingRequestErrors.Add(E);
                    db.SaveChanges();
                }
            } catch (Exception e)
            {

                LogErrorRaw("Error occurred when logging an error.  Original error: " + psModule + ": " + psMessage + " : " + e.ToString(), "Log.LogError");

            }      
        }

        public static void LogErrorRaw(string psMessage, string psModule)
        {
                string sSource= "OrderImportClasses";
                string sLog="Application";
                string sEvent=psMessage;

                if (!EventLog.SourceExists(sSource))                    
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Error);

        }
    }
}
