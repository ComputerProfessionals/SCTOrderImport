using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace OrderImportClasses
{
    public static class Util
    {
        public static string AppSettingValue(string psKey)
        {
            string lsResult = "";

            AppSettingsReader asr = new AppSettingsReader();
            {
                if (asr.GetValue(psKey, Type.GetType("System.String")) != null)
                {
                    lsResult = asr.GetValue(psKey, Type.GetType("System.String")).ToString();
                }
            }

            return lsResult;
        }

        public static bool GetBoolean(Object value)
        {
            if (value == null || value == DBNull.Value)
                return false;
            else {
                bool b = false;
                bool.TryParse(value.ToString(), out b);
                return b;
            }               
        }

        public static string GetString(Object value)
        {
            if (value != null && value != DBNull.Value)
                return value.ToString();
            else
                return "";
        }

        public static decimal GetDecimal(Object value)
        {
            if (value != null && value != DBNull.Value)
            {
                decimal d = 0;
                decimal.TryParse(value.ToString(), out d);
                return d;
            }
            else
                return 0;
        }
    }
}
