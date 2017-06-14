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


namespace OrderImportServiceDev
{
    public partial class OrderImport : ServiceBase
    {
        System.Threading.Timer timer;
        bool polling = false;
      
        public OrderImport()
        {
            InitializeComponent();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Console.WriteLine("MyHandler caught : " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            Log.LogErrorRaw(e.Message, "OrderImportServiceDev");
        }


        protected override void OnStart(string[] args)
        {

            Start();
            
        } 
        
        private void Start()
        {

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            int liInt = 5000;

            object value = System.Configuration.ConfigurationManager.AppSettings["TimerInterval"];

            if (value != null)
                Int32.TryParse(value.ToString(), out liInt);

            TimeSpan tsInterval = new TimeSpan(0, 0, liInt);
            timer = new System.Threading.Timer(new System.Threading.TimerCallback(DoPoll) , null, tsInterval, tsInterval); 
        }        
      

        protected override void OnStop()
        {                                  
            timer.Dispose();           
        }      

        private void DoPoll(object state)
        {
           
            bool started = false;

            try
            {
                
                if (Util.GetBoolean(Util.AppSettingValue("DebugMessages")))
                    Log.LogError("Enter DoPoll", "OrderImportServiceDev.DoPoll");

                if (!polling)
                {

                    started = true;
                    polling = true;

                    if (Util.GetBoolean(Util.AppSettingValue("DebugMessages")))
                        Log.LogError("Call ProcessOrders.Process()", "OrderImportServiceDev.DoPoll", 0);

                    ProcessOrders loPO = new ProcessOrders();
                    string lsMessage = "";
                    bool lbResult = loPO.Process(out lsMessage);
                    polling = false;                   
                    
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString(), "OrderImportServiceDev.DoPoll", 0);
            }
            finally {
                if (started==true && polling==true)
                    polling = false;
            }
        } 
       
    }
   
}
