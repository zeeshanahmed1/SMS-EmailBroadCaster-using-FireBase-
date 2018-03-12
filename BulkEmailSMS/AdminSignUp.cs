using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using iTextSharp;
using iTextSharp.text.factories;
using iTextSharp.text.html;
//using iTextSharp.text.rtf;
using System.Data.SqlClient;
using System.Net.Mail;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using GemBox.Spreadsheet;
using System.Net.Http;
using FirebaseNet.Database;

namespace BulkEmailSMS
{
    public partial class AdminSignUp : Form
    {
        public AdminSignUp()
        {
            InitializeComponent();
        }

        FirebaseDB firebaseDB = new FirebaseDB("https://viscous-d6aa3.firebaseio.com/");
        Contacts objStudent = new Contacts();

        private void btnCreateAdmin_Click(object sender, EventArgs e)
        {

          
            try
            {
                if (tbFirstName.Text.Trim() == "" && tbAddress.Text == "" && tbEmail.Text == "")
                {
                    MessageBox.Show("Please fill all the fields");
                }
                else
                {
                  

                    objStudent.Name = tbFirstName.Text.ToString();
                    objStudent.Email = tbEmail.Text.ToString();
                    objStudent.Address = tbAddress.Text.ToString();
                    objStudent.Phone = mtbMobileNo.Text.ToString();
                    objStudent.Category = "Admin";
                   this.JSONSerialize(objStudent);

                    FirebaseResponse postResponse = firebaseDB.Post(this.JSONSerialize(objStudent));

                    MessageBox.Show("Saved");

                    //loadRawDataTable();
                  //  dgvContactView.DataSource = loadRawDataTable();
                 //   LoadSpinner();

                 //   clearForm();



                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }




        }


        public String JSONSerialize(Contacts objStudent)
        {
            String js;
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(Contacts));
            jsonSer.WriteObject(stream, objStudent);
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);


            return js = sr.ReadToEnd();
        }

        public void designbtns()
        {

            btnCreateAdmin.TabStop = false;
            btnCreateAdmin.FlatStyle = FlatStyle.Flat;
            btnCreateAdmin.FlatAppearance.BorderSize = 0;
            btnCreateAdmin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnCreateAdmin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
        }

        private void AdminSignUp_Load(object sender, System.EventArgs e)
        {
            designbtns();
        }

        private void tbFirstName_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void label3_Click(object sender, System.EventArgs e)
        {

        }

        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        private void tbEmail_TextChanged(object sender, System.EventArgs e)
        {

        }

    }
}
