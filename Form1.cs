using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REST.Service {
    public partial class Form1 : Form {
        public SQLWebService sqlWebService = new SQLWebService();
        public Form1() {
            InitializeComponent();
            sqlWebService.ListenerPrefix = "http://*:80/SQLServer/";
            sqlWebService.ConnectionString = "Data Source=192.168.5.6;Initial Catalog=AdventureWorks;User ID=sa;Password=password";
            textConnectionString.Text = sqlWebService.ConnectionString;
            textListenerPrefix.Text = sqlWebService.ListenerPrefix;
            textRecordsPerPage.Text = sqlWebService.RecordsPerPage.ToString();
        }

        private void buttonStartWebService_Click(object sender, EventArgs e) {
            try {
                sqlWebService.StartWebServer();
            }
            catch (Exception exception) {
                MessageBox.Show(exception.ToString());
            }
            System.Threading.Thread.Sleep(1500);
            buttonStartWebService.Text = sqlWebService.HttpListener != null && sqlWebService.HttpListener.IsListening 
                ? "Stop Web Service" : "Start Web Service";
        }

        private void buttonConnect_Click(object sender, EventArgs e) {
            try {
                if (sqlWebService.Connection.State == ConnectionState.Open)
                    sqlWebService.Connection.Close();
                else
                    sqlWebService.Connect();
            }
            catch(Exception exception) {
                MessageBox.Show(exception.ToString());
            }
            buttonConnect.Text = sqlWebService.Connection.State == ConnectionState.Open  ?  "Disconnect" : "Connect";
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void connectionStringTextBox_TextChanged(object sender, EventArgs e) {
            sqlWebService.ConnectionString = textConnectionString.Text;
        }

        private void textListenerPrefix_TextChanged(object sender, EventArgs e) {
            sqlWebService.ListenerPrefix = textListenerPrefix.Text;
        }

        private void textRecordsPerPage_TextChanged(object sender, EventArgs e) {
            try {
                sqlWebService.RecordsPerPage = Convert.ToInt32(textRecordsPerPage.Text);
            }
            catch { }
        }


    }
}
