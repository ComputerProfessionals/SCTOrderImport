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
using DataModel;


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

       
    }

   
}
