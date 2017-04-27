using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using OrderImportClasses;


namespace OrderImportService
{
    public partial class OrderImport : ServiceBase
    {
        System.Timers.Timer timer;
        bool polling = false;
        public OrderImport()
        {
            try {
                InitializeComponent();
            } catch(Exception e)
            {
                Log.LogError(e.ToString(), "OrderImport.OrderImport");
            }
           
        }

        protected override void OnStart(string[] args)
        {
            try {

                AppDomain.CurrentDomain.UnhandledException += errHandler;


              //  Log.LogError("Service Starting", "OrderimportService.OnStart");
                double liInt = 5000;

                object value = System.Configuration.ConfigurationManager.AppSettings["TimerInterval"];

                if (value != null)
                    double.TryParse(value.ToString(), out liInt);

                timer = new System.Timers.Timer(liInt);

                // Log.LogError("Timer interval = " + liInt.ToString(), "OrderimportService.OnStart");

                timer.Enabled = true;
                timer.Elapsed += DoPoll;               
                timer.Start();

                //Log.LogError("Timer started", "OrderimportService.OnStart");

            } catch(Exception e) {
                Log.LogError(e.ToString(), "OrderImportService.OnStart");
            }
            
        }  
        
        private void errHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            try
            {
                Log.LogError(ex.ToString(), "OrderImportService.UnhandledException");
            }
            catch (Exception ex1) {

            }
        }     

        protected override void OnStop()
        {
           // Log.LogError("Service Stopping", "OrderimportService.OnStop");            
            timer.Dispose();
        }      

        private void DoPoll(object sender, EventArgs e)
        {
            //  Log.LogError("Enter DoPoll", "OrderImportService.DoPoll");
            bool started = false;

            try
            {
                if (!polling)
                {

                    polling = true;
                    started = true;
                  //  Log.LogError("Call ProcessOrder()", "OrderImportService.DoPoll");
                    ProcessOrders loPO = new ProcessOrders();
                    string lsMessage = "";
                    bool lbResult = loPO.Process(out lsMessage);
                    polling = false;

                    if (!lbResult)
                    {
                        SendErrorEmails(lsMessage);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString(), "OrderImportService.DoPoll");
            }
            finally {
                if (started==true && polling==true)
                    polling = false;
            }
        }

        private void SendErrorEmails(string psMessage)
        {
            try {
                DataTable dt = (new DataHelper()).GetDataTable("select * from SiteConfig");

                string lsRecipients = AppSettingValue("ErrorRecipients");

                foreach (string lsTo in lsRecipients.Split(';'))
                {
                    if (lsTo.Trim() != "")
                    {

                        MailHelper mh = new MailHelper();                       
                          
                        mh.Body = 
                            "<div style='font-family:arial;font-size:10pt'>" + 
                                "<div style='font-size:13pt;font-weight:bold'>" + 
                                    "The following error occurred when attempting to save an online customer order:" + 
                                "</div>" + 
                                "<div style='padding:20px;'>" + psMessage + "</div>" + 
                            "</div>";
                        mh.HostName = GetString(dt.Rows[0]["SMTPServer"]); // "MELSVEX3046.SCTLOGISTICS.COM.AU"; //lsHostName;

                        mh.FromEmail = GetString(dt.Rows[0]["SMTPEmail"]); // "admin_noreply@sctlogistics.com";
                        mh.IsHtmlBody = true;
                        mh.ToEmail = lsTo;
                        mh.SiteName = GetString(dt.Rows[0]["SiteName"]);
                        mh.SMTPEmail = GetString(dt.Rows[0]["SMTPEmail"]);
                        mh.SMTPPassword = GetString(dt.Rows[0]["SMTPPassword"]);
                        mh.Subject = AppSettingValue("ErrorSubject");
                        mh.SMTPPort = 0;
                        mh.SendEmail();
                    }

                }   
            }
            catch (Exception e) {
                Log.LogError(e.ToString(), "OrderImportService.SendErrorEmails");
            }
        }

        private string GetString(object poValue)
        {
            if (poValue == null || poValue.Equals(DBNull.Value))
                return ""; 
            else
                return poValue.ToString();
        }

        private string AppSettingValue(string psKey)
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
    }

   
}
