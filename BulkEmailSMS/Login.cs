using FirebaseNet.Database;
using iTextSharp.text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BulkEmailSMS
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        FirebaseDB firebaseDB = new FirebaseDB(ConfigurationManager.ConnectionStrings["Link"].ConnectionString);
        DataTable cTable;
        DataTable tblFiltered;
        private void Login_Load(object sender, EventArgs e)
        {
            designbtns();

        }
        public void designbtns()
        {

            btnLogin.TabStop = false;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (tbFirstName.Text.Trim() == "" && tbEmail.Text == "")
            {
                MessageBox.Show("Please fill all the fields");
            }
            else
            {
                new MailAddress(tbEmail.Text.Trim());

                if (loadRawDataTable())
                {
                   mainForm mf = new mainForm();
                   mf.Show();
                   this.Hide();
                }
                else
                {
                    MessageBox.Show("Login Failed");
                }

               

              

            }
        }

        public bool loadRawDataTable()
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
                             .Where(r => r.Field<string>("Category") == "Admin")
                             .CopyToDataTable();





            string name = tbFirstName.Text.Trim();
            string email = tbEmail.Text.Trim();

            bool contains = tblFiltered.AsEnumerable().Any(row => name == row.Field<String>("Name")) && tblFiltered.AsEnumerable().Any(row => email == row.Field<String>("Email"));


            return contains;

        //    var result = tblFiltered.Rows.Find(tbEmail.Text);

        }
    }
}