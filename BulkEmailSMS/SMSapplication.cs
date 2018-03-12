using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using FirebaseNet.Database;
using System.Web;
using System.Web.Script.Serialization;
using GemBox.Spreadsheet;
using System.Net.Http;

using System.Data;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;

namespace BulkEmailSMS
{
    public partial class SMS : Form
    {

       
        int b = 0;
        string cat = null;
        public SMS()
        {
            InitializeComponent();
        }
        DataTable cTable;
        DataTable tblFiltered;
        FirebaseDB firebaseDB = new FirebaseDB("https://viscous-d6aa3.firebaseio.com/");
        SerialPort port = new SerialPort();
        clsSMS objclsSMS = new clsSMS();
        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        

        
        private void WriteStatusBar(string status)
        {
            try
            {
                statusBar1.Text = "Message: " + status;
            }
            catch (Exception ex)
            {

            }
        }
        

      

        private void SMSapplication_Load(object sender, EventArgs e)
        {
            btnDesign();
            loadRawDataTable();
            dgvSmsContactView.DataSource = loadRawDataTable();
            
            LoadSpinner();
            try
            {
              //  dgvSmsContactView.DataSource = cn.GetData(@"SELECT ContactName AS [ContactName], Email AS [Email], Phone AS [Mobile No.], Address AS [Address]  FROM Contacts ORDER BY ContactName ASC");


                #region Display all available COM Ports
                string[] ports = SerialPort.GetPortNames();

                // Add all port names to the combo box:
                foreach (string port in ports)
                {
                    this.cboPortName.Items.Add(port);
                }
                #endregion

                //Remove tab pages
                this.tabSMSapplication.TabPages.Remove(tbSendSMS);


                this.btnDisconnect.Enabled = false;
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //Open communication port 
                this.port = objclsSMS.OpenPort(this.cboPortName.Text, Convert.ToInt32(this.cboBaudRate.Text), Convert.ToInt32(this.cboDataBits.Text), Convert.ToInt32(this.txtReadTimeOut.Text), Convert.ToInt32(this.txtWriteTimeOut.Text));

                if (this.port != null)
                {
                    this.gboPortSettings.Enabled = false;

                    //MessageBox.Show("Modem is connected at PORT " + this.cboPortName.Text);
                    this.statusBar1.Text = "Modem is connected at PORT " + this.cboPortName.Text;

                    //Add tab pages
                    this.tabSMSapplication.TabPages.Add(tbSendSMS);


                    this.lblConnectionStatus.Text = "Connected at " + this.cboPortName.Text;
                    this.btnDisconnect.Enabled = true;
                }

                else
                {
                    //MessageBox.Show("Invalid port settings");
                    this.statusBar1.Text = "Invalid port settings";
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }

        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                this.gboPortSettings.Enabled = true;
                objclsSMS.ClosePort(this.port);

                //Remove tab pages
                this.tabSMSapplication.TabPages.Remove(tbSendSMS);


                this.lblConnectionStatus.Text = "Not Connected";
                this.btnDisconnect.Enabled = false;

            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {
            
            
        }




   

        #region Error Log
        public void ErrorLog(string Message)
        {
            StreamWriter sw = null;

            try
            {
                WriteStatusBar(Message);

                string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                //string sPathName = @"E:\";

                // start

                string appFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

                string sPathName = Path.Combine(
                Directory.GetParent(appFolderPath).Parent.Name, @"Resources\SMSapplicationErrorLog_\");




                //end




               // string sPathName = @"SMSapplicationErrorLog_";

                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();

                string dateTime = DateTime.Now.ToString();


               // string sErrorTime = sDay + "-" + sMonth + "-" + sYear;

                string sErrorTime = dateTime;

                sw = new StreamWriter(sPathName + sErrorTime + ".txt", true);

                sw.WriteLine(sLogFormat + Message);
                sw.Flush();

            }
            catch (Exception ex)
            {
                //ErrorLog(ex.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                    sw.Close();
                }
            }

        }
        #endregion

        private void dgvSmsContactView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow dgv in dgvSmsContactView.Rows)
            {
                dgv.Cells["chk"].Value = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(5);

            try
            {
                b = 0;
                // selects contacts
                pbrSendSms.Refresh();
                string selected = string.Empty;
                foreach (DataGridViewRow row in dgvSmsContactView.Rows)
                {

                    bool isSelected = Convert.ToBoolean(row.Cells["chk"].Value);
                    if (isSelected)
                    {

                        b += 1;
                        selected += Environment.NewLine;
                        selected += row.Cells["Phone"].Value.ToString();

                    }
                }

                //spilits contacts
                char[] delimiters = new char[] { '\r', '\n' };
                string[] words = selected.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < words.Length; i++)
                {
                      // code here

                        Thread.Sleep(5000);

                    if (objclsSMS.sendMsg(this.port, words[i], this.rTxtMessage.Text))
                    {
                        pbrSendSms.Minimum = 0;
                        pbrSendSms.Maximum = b;
                        int c = i + 1;
                        pbrSendSms.Value = c;
                        this.statusBar1.Text = "Message has sent successfully";

                    }
                    else
                    {
                        this.statusBar1.Text = "Failed to send message";
                    }


                }
                    
                }

            
            
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
            
         
        }

        public void btnDesign()
        {
            btnSendSMS.TabStop = false;
            btnSendSMS.FlatStyle = FlatStyle.Flat;
            btnSendSMS.FlatAppearance.BorderSize = 0;
            btnSendSMS.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSendSMS.FlatAppearance.MouseOverBackColor = Color.Transparent;

            
            btnDisconnect.TabStop = false;
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.FlatAppearance.BorderSize = 0;
            btnDisconnect.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnDisconnect.FlatAppearance.MouseOverBackColor = Color.Transparent;

            btnOK.TabStop = false;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnOK.FlatAppearance.MouseOverBackColor = Color.Transparent;

            button1.TabStop = false;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;

           

            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
           
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            mainForm f3 = new mainForm();
            f3.Show();
            this.Hide();

        }

        private void dgvSmsContactView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow row = dgvSmsContactView.Rows[e.RowIndex];
            row.DefaultCellStyle.BackColor = System.Drawing.Color.Thistle;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            mainForm f3 = new mainForm();
            f3.Show();
            this.Hide();
        }

        private void cbCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            cat = cbCat.SelectedValue.ToString().Trim();

            loadContactsDgvByCat(cat);
        }
        public void loadContactsDgvByCat(string catName)
        {
            DataTable tblFiltered = cTable.AsEnumerable()
                             .Where(r => r.Field<string>("Category") == catName)
                             .CopyToDataTable();
            dgvSmsContactView.DataSource = tblFiltered;
            

        }



        public DataTable loadRawDataTable()
        {


            FirebaseResponse getResponse = firebaseDB.Get();
            Console.WriteLine(getResponse.Success);

            if (getResponse.Success)
            {



                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, Contacts>>(getResponse.JSONContent);



                cTable = new DataTable();
                cTable = DictionaryToDataTable.ConvertTo<Contacts>(dictionary, "DemoTable");





            }

            tblFiltered = cTable.AsEnumerable()
                             .Where(r => r.Field<string>("Category") != "Admin")
                             .CopyToDataTable();
            dgvSmsContactView.DataSource = tblFiltered;
            
            return tblFiltered;
        }

        public void LoadSpinner()
        {


            cbCat.Items.Clear();

            cbCat.ValueMember = "Category";
            cbCat.DisplayMember = "Category";



            cbCat.Items.AddRange(tblFiltered.AsEnumerable().Select(i => i.ItemArray[5]).Distinct().ToArray());

        }
       

    }
}