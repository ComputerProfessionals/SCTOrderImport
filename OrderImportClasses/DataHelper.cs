﻿using System;
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

                using (SCTModel db = new SCTModel())
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
                Log.LogError(e.ToString(), "OrderImportClasses.DataHelper.GetDataTable()");
            }
           
            return dt;

        }
    }
}
