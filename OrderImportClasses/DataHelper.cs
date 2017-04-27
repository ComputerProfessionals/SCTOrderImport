using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace OrderImportClasses
{
    public class DataHelper
    {
        public DataTable GetDataTable(string psSql)
        {
            DataTable dt = new DataTable();

            try {

                string conStr = ConfigurationManager.ConnectionStrings["SCTModel"].ToString();

                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = psSql;
                    dt.Load(cmd.ExecuteReader());
                }
                 
            }
            catch (Exception e) {

            }
           
            return dt;

        }
    }
}
