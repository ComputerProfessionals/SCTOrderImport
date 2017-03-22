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
        
        public OrderImport()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            int liInt = 5000;
            string value = System.Configuration.ConfigurationManager.AppSettings["TimerInterval"];
            int.TryParse(value, out liInt);

            objTimer.Interval = liInt;
            objTimer.Tick += DoPoll;         
            objTimer.Start();
        }       

        protected override void OnStop()
        {

        }

        private void DoPoll(object sender, EventArgs e)
        {

            ProcessOrders loPO = new ProcessOrders();            
            loPO.Process();
        }
    }
}
