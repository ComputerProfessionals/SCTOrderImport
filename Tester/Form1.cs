﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrderImportClasses;
using System.Configuration;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try {
                ProcessOrders p = new ProcessOrders();
                p.EncryptConfigSection("appSettings");
                p.EncryptConfigSection("connectionStrings");
            } catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
           
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            ProcessOrders po = new ProcessOrders();
            string lsResult = "";

            po.Process(out lsResult);
            
            MessageBox.Show("Orders have been processed. " + lsResult);          

            this.Cursor = Cursors.Default;

        }       

      
        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ProcessOrders p = new ProcessOrders();            
                p.DecryptConfigSection("appSettings");
                p.DecryptConfigSection("connectionStrings");
              
                MessageBox.Show("Config file has been decrypted.");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            this.Cursor = Cursors.Default;



        }
    }
}
