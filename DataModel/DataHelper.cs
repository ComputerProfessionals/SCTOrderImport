using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DataModel
{
    public class DataHelper
    {
        public DataTable GetDataTable(string psSql)
        {
            DataTable dt = new DataTable();

            try {

                using (SCT db = new SCT())
                {

                    string conStr = db.Database.Connection.ConnectionString;

                    using (SqlConnection conn = new SqlConnection(conStr))
                    {

                        SqlCommand cmd = new SqlCommand(psSql, conn);

                        using (SqlDataAdapter ad = new SqlDataAdapter(cmd))

                        {

                            ad.SelectCommand.CommandType = CommandType.Text;
                            ad.Fill(dt);

                        }

                    }  
                }                  
                 
            }
            catch (Exception e) {
                LogError(e.ToString(), "OrderImportClasses.DataHelper.GetDataTable()");
            }
           
            return dt;

        }

        public  void LogError(string psMessage, string psModule)
        {
            ShippingRequestError E = new ShippingRequestError();// new ShippingRequestError();
            E.DateCreated = System.DateTime.Now;
            E.Module = psModule;
            E.ErrorDetail = psMessage;
            using (SCT db = new SCT())
            {
                db.ShippingRequestErrors.Add(E);
                db.SaveChanges();
            }
        }
    }
}
