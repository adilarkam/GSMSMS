using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace ReceiveSMS
{
    public partial class Form1 : Form
    {
        SerialPort port = new SerialPort();
        SmsHelper smsHelper = new SmsHelper();
        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();

        public Form1()
        {
            InitializeComponent();



            //ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            // Add all port names to the combo box:
            foreach (string port in ports)
            {
                this.cmbPort.Items.Add(port);
            }

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnConnect.Text == "&Connect")
                {
                    //Open communication port 
                    this.port = smsHelper.OpenPort(cmbPort.Text, 9600, 8, 300, 300);

                    //Delete all existing SMS
                    //smsHelper.DeleteMessage(this.port, "AT+CMGD=1,4");


                    timer.Enabled = true;
                    btnConnect.Text = "&Disconnect";
                    stTxt.BackColor = Color.LightGreen;
                    stTxt.Text = "Connected";

                }
                else
                {
                    

                    timer.Enabled = false;
                    btnConnect.Text = "&Connect";
                    smsHelper.ClosePort(this.port);
                    stTxt.Text = "Disconnected";
                    stTxt.BackColor = Color.Orange;

                }

            }
            catch (Exception ex)
            {
                stTxtErr.Text = ex.Message;

            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {

            try
            {
                objShortMessageCollection = smsHelper.ReadSMS(this.port, "AT+CMGL=\"ALL\"");

                foreach (ShortMessage msg in objShortMessageCollection)
                {
                    txtSMS.Text += msg.Message + "\r\n";
                }

                smsHelper.DeleteMessage(this.port, "AT+CMGD=1,4");

            }
            catch(Exception ex)
            {
                stTxtErr.Text = ex.Message;

            }

        }
    }
}
