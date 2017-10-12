using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainProject
{
    public partial class GSM : Form
    {

        long NimasPhoneNumber;

        //Ctrl-z:   New Line:           (Char)26
        //CR:       Carriage Return:    (char)13

        public GSM()
        {
            InitializeComponent();

            this.NimasPhoneNumber = 989125185224;
        }

        private void GSM_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {
                serialPort1.Open();
            }

            CallANumber();

            serialPort1.Close();
        }

        private void CallANumber()
        {
            // Call a number: "ATD98912xxxxxxx;<Ctrl-z>"
            //this.serialPort1.Write("ATD" + NimasPhoneNumber + ";" + (char)26);

            int j = 5;

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            MessageBox.Show(serialPort1.ReadExisting());
        }

        private void AT()
        {
            string command = "AT";

            serialPort1.Write(command + System.Environment.NewLine);
        }

        private void SMSMode()
        {
            //OUTPUT: +CMGF: (0,1)
            string command = "AT+CMGF=?";

            serialPort1.Write(command + System.Environment.NewLine);

            //IF remove this line; Next OUTPUT will be: +CMGF: 0
            serialPort1.Write("AT+CMGF=1" + System.Environment.NewLine);

            //OUTPUT: +CMGF: 1
            command = "AT+CMGF?";

            serialPort1.Write(command + System.Environment.NewLine);
        }

        private void BatteryCharge()
        {
            string command = "AT+CBC?";

            serialPort1.Write(command + System.Environment.NewLine);
        }

        private void ServiceCenterAddress()
        {
            string command = "AT+CSCA?";

            //OUTPUT: +CSCA: "+9891100500",145
            serialPort1.Write(command + System.Environment.NewLine);
        }

        private void generalATCommands()
        {
            //نام کارخانه سازنده گوشی
            string manufacture = "AT+CGMI";
            serialPort1.Write(manufacture + System.Environment.NewLine);

            System.Threading.Thread.Sleep(2000);

            //مدل گوشی
            string model = "at+cgmm";
            serialPort1.Write(model + System.Environment.NewLine);

            System.Threading.Thread.Sleep(2000);

            //میزان شارژ باطری گوشی
            string battary = "AT+CBC";
            serialPort1.Write(battary + System.Environment.NewLine);

            System.Threading.Thread.Sleep(3000);

            //حد اکثر مقدار برای آنتن دهی گوشی
            string maxAntena = "AT+CSQ=?";
            serialPort1.Write(maxAntena + System.Environment.NewLine);


            System.Threading.Thread.Sleep(5000);

            //میزان آنتن دهی گوشی
            string antena = "AT+CSQ";
            serialPort1.Write(antena + System.Environment.NewLine);

            System.Threading.Thread.Sleep(5000);

        }

        private void sendSMSATCommands()
        {

            ///
            //MY PHONE DOES NOT SUPPORT SMS TEXT MODE
            ///
            //Message format: TEXT OR PDU
            //serialPort1.Write("AT+CMGF=1" + (char)13);

            System.Threading.Thread.Sleep(1000);

            //Message Center Number
            serialPort1.Write("AT+CSCA=\"" + "+9891100500" + "\"" + (char)13);

            System.Threading.Thread.Sleep(1000);

            //Send
            serialPort1.Write("AT+CMGS=\"" + "09123976634" + "\"" + (char)13);

            System.Threading.Thread.Sleep(100);

            //
            serialPort1.Write("SMS Message Sending Test!" + (char)26);

            System.Threading.Thread.Sleep(100);

        }

        private void SMSMessageService()
        {
            //P.144
            string command = "AT+CSMS=0";

            serialPort1.Write(command + System.Environment.NewLine);
        }

    }
}
