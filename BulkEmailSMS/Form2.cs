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
using FirebaseNet.Database;
using System.Web;
using System.Web.Script.Serialization;
using GemBox.Spreadsheet;
using System.Net.Http;
using System.Data.OleDb;
using System.Data.Common;
using System.Configuration;
using Newtonsoft.Json;




namespace BulkEmailSMS
{

     
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
       
       
        DataTable tblFiltered;
        DataTable cTable;
        int ID = 0;
        string appPath;
        string pass = null;
       // FirebaseDB firebaseDB = new FirebaseDB("https://viscous-d6aa3.firebaseio.com/");

     
        FirebaseDB firebaseDB = new FirebaseDB(ConfigurationManager.ConnectionStrings["Link"].ConnectionString);
        
        Contacts objStudent = new Contacts();
       
        private void btnAdd_Click(object sender, EventArgs e)
        {

            String category;
            try
            {
                if (tbFirstName.Text.Trim() == "" && tbAddress.Text == "" && tbEmail.Text == "" && tbCat.Text == "")
                {
                    MessageBox.Show("Please fill all the fields");
                }
                else
                {

                   new MailAddress(tbEmail.Text.Trim());
                   if (tbCat.Text == "Admin")
                   {
                       MessageBox.Show("Please fill correct values in the fields");

                   }
                    else

                   
                    objStudent.Name = tbFirstName.Text.ToString();
                    objStudent.Email = tbEmail.Text.ToString();
                    objStudent.Address = tbAddress.Text.ToString();
                    objStudent.Phone = mtbMobileNo.Text.ToString();
                    objStudent.Category = tbCat.Text.ToString();
                    this.JSONSerialize(objStudent);

                    FirebaseResponse postResponse = firebaseDB.Post(this.JSONSerialize(objStudent));

                    MessageBox.Show("Saved");

                    loadRawDataTable();
                    dgvContactView.DataSource = loadRawDataTable();
                    LoadSpinner();

                   // clearForm();
                    


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }




          

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            desingbtns();
            loadRawDataTable();
            dgvContactView.DataSource = loadRawDataTable();
            LoadSpinner();



           // DataGridViewImageButtonDeleteColumn delColumn = new DataGridViewImageButtonDeleteColumn();
            DataGridViewButtonColumn delColumn = new DataGridViewButtonColumn();
            delColumn.Name = "btnDgvDelete";
            delColumn.HeaderText = " ";
            delColumn.Text = "Delete";
            delColumn.UseColumnTextForButtonValue = true;
            delColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            delColumn.FlatStyle = FlatStyle.Standard;
            dgvContactView.Columns.Add(delColumn);

           // DataGridViewImageButtonEditColumn editRow = new DataGridViewImageButtonEditColumn();
            DataGridViewButtonColumn editRow = new DataGridViewButtonColumn();
            editRow.Name = "btnDgvEdit";
            editRow.HeaderText = " ";
            editRow.Text = "Edit";
            editRow.UseColumnTextForButtonValue = true;
            editRow.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            editRow.FlatStyle = FlatStyle.Standard;
            editRow.Text = "Edit";
            
            dgvContactView.Columns.Add(editRow);



          //  dgvContactView.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
           // dgvContactView.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
            dgvContactView.DataSource = tblFiltered;
            return tblFiltered;
        }

        public void LoadSpinner()
        {


            cbCat.Items.Clear();

            cbCat.ValueMember = "Category";
            cbCat.DisplayMember = "Category";



            cbCat.Items.AddRange(tblFiltered.AsEnumerable().Select(i => i.ItemArray[5]).Distinct().ToArray());
        
        }
       
       

       
      

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string cat;
            try
            {

                if (tbCat.Text == "")
                {
                    cat = cbCat.SelectedValue.ToString();
                }
                else
                {
                    cat = tbCat.Text.Trim();
                }


                new MailAddress(tbEmail.Text.Trim());

                objStudent.Name = tbFirstName.Text.ToString();
                objStudent.Email = tbEmail.Text.ToString();
                objStudent.Address = tbAddress.Text.ToString();
                objStudent.Phone = mtbMobileNo.Text.ToString();
                objStudent.Category = cat;
                this.JSONSerialize(objStudent);

                string id = objStudent.ID.ToString();
                FirebaseDB firebaseDBTeams = firebaseDB.Node(id);

                FirebaseResponse putResponse = firebaseDBTeams.Put(this.JSONSerialize(objStudent));
                if (putResponse.Success)
                {
                    MessageBox.Show("Successfull");
                    loadRawDataTable();
                    dgvContactView.DataSource = loadRawDataTable();
                    LoadSpinner();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dgvContactView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                var senderGrid = (DataGridView)sender;
               
                if (e.ColumnIndex == senderGrid.Columns["btnDgvEdit"].Index && e.RowIndex >= 0)
                {
                    // dissble add back and close buttons
                    this.btnAdd.Enabled = false;
                    tbFirstName.Text = dgvContactView.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                    mtbMobileNo.Text = dgvContactView.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
                    tbAddress.Text = dgvContactView.Rows[e.RowIndex].Cells["Address"].Value.ToString();
                    tbEmail.Text = dgvContactView.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                    tbCat.Text = dgvContactView.Rows[e.RowIndex].Cells["Category"].Value.ToString();

                   
                    string ID = dgvContactView.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                    objStudent.ID = dgvContactView.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                 
                   



                }
                else if (e.ColumnIndex == senderGrid.Columns["btnDgvDelete"].Index && e.RowIndex >= 0)
                {

                  string  ID = dgvContactView.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                    // Referring to Node   
                    FirebaseDB firebaseDBTeams = firebaseDB.Node(ID);  
  
                    FirebaseResponse deleteResponse = firebaseDBTeams.Delete();
                    if (deleteResponse.Success)
                    {
                        MessageBox.Show("Deleted");

                        loadRawDataTable();
                        dgvContactView.DataSource = loadRawDataTable();
                        LoadSpinner();
                    }
                

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void clearForm()
        {
            mtbMobileNo.Clear();
            tbFirstName.Clear();
            tbEmail.Clear();
            tbAddress.Clear();
            tbCat.Clear();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainForm f3 = new mainForm();
            f3.Show();
            this.Hide();
        }

        public void desingbtns()
        {
            btnBack.TabStop = false;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;

            btnAdd.TabStop = false;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;

            btnUpdate.TabStop = false;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnUpdate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;

            btnClose.TabStop = false;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;


            btnGeneratePdf.TabStop = false;
            btnGeneratePdf.FlatStyle = FlatStyle.Flat;
            btnGeneratePdf.FlatAppearance.BorderSize = 0;
            btnGeneratePdf.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnGeneratePdf.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;


            btnImport.TabStop = false;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnImport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;


        }

        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {




            string sSelectedPath = null;
           

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            ExcelFile ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("DataTable to Sheet");

          
          //  ws.Cells[0, 0].Value = "DataTable insert example:";

            // Insert DataTable into an Excel worksheet.
            ws.InsertDataTable(loadRawDataTable(), new InsertDataTableOptions()
                {
                    ColumnHeaders = true,
                  //  StartRow = 2
                });

           // ef.Save("DataTable to Sheet.xlsx");


            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Custom Description";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                 sSelectedPath = fbd.SelectedPath + "\\Viscous_info.xlsx";
            }

           ef.Save(sSelectedPath);

           

            MessageBox.Show("Export Completed");



        }

        private void dgvContactView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow row = dgvContactView.Rows[e.RowIndex];
            row.DefaultCellStyle.BackColor = System.Drawing.Color.Thistle;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
           

            OpenFileDialog opFile = new OpenFileDialog();
            opFile.Title = "Select a File";
            //opFile.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";

            string appFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            string resourcesFolderPath = Path.Combine(
            Directory.GetParent(appFolderPath).Parent.Name, @"Resources\");



            if (Directory.Exists(resourcesFolderPath) == false)
            {
                Directory.CreateDirectory(resourcesFolderPath);
            }
            if (opFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string file = opFile.FileName;
                    //  string adddress = resourcesFolderPath;

                    InsertExcelRecords(file);



                }
                catch (Exception exp)
                {
                    MessageBox.Show("Unable to open file >>> " + exp.Message);
                }
            }
            else
            {
                opFile.Dispose();
            }


        }
        public void InsertExcelRecords(string path)
        {

          //  var pathToExcel = @"C:\Users\Zeeshan\Desktop\Viscous_info.xlsx";
            var sheetName = "Sheet1";
           // var destinationPath = @"C:\Users\Zeeshan\Desktop\file.json";

            //Use this connection string if you have Office 2007+ drivers installed and 
            //your data is saved in a .xlsx file
            var connectionString = String.Format(@"
                Provider=Microsoft.ACE.OLEDB.12.0;
                Data Source={0};
                Extended Properties=""Excel 12.0 Xml;HDR=YES""
            ", path);

            //Creating and opening a data connection to the Excel sheet 
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = String.Format(
                    @"SELECT * FROM [{0}$]",
                    sheetName
                );
                
                using (var rdr = cmd.ExecuteReader())
                {
                   
                    //LINQ query - when executed will create anonymous objects for each row
                    var query =
                    (from DbDataRecord row in rdr
                     select row).Select(x =>
                     {

                         objStudent.Name = x[1].ToString();
                         objStudent.Phone = x[2].ToString();
                         objStudent.Email = x[3].ToString();
                         objStudent.Address = x[4].ToString();

                         objStudent.Category = x[5].ToString();
                         this.JSONSerialize(objStudent);

                         FirebaseResponse postResponse = firebaseDB.Post(this.JSONSerialize(objStudent));

                         this.statusBar1.Text = "Importing...";
                         //dynamic item = new ExpandoObject();
                         Dictionary<string, object> item = new Dictionary<string, object>();
                         item.Add(rdr.GetName(1), x[1]);
                         item.Add(rdr.GetName(2), x[2]);
                         item.Add(rdr.GetName(3), x[3]);
                         item.Add(rdr.GetName(4), x[4]);
                         item.Add(rdr.GetName(5), x[5]);
                          return item;



                     });
                    //{"Address":"khi","Category":"Faculty","Email":"abc@gmail.com","ID":null,"Name":"abc","Phone":"00000000000"}

                   
                    var json = JsonConvert.SerializeObject(query);
                    //Write the file to the destination path    
                    
                    //  File.WriteAllText(destinationPath, json);
                }
            }


            this.statusBar1.Text = "Importing Completed";

        }


        private void cbCat_SelectedIndexChanged(object sender, EventArgs e)
        {

                pass = cbCat.SelectedItem.ToString();
                if (pass != "")
                {
                    loadContactsDgvByCat(pass);
                }
        }

        public void loadContactsDgvByCat(string catName)
        {
            DataTable tblFiltered = cTable.AsEnumerable()
                              .Where(r => r.Field<string>("Category") == catName)
                              .CopyToDataTable();
            dgvContactView.DataSource = tblFiltered;
        }


        public String JSONSerialize(Contacts objStudent)
        {
            String js;
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(Contacts));
            jsonSer.WriteObject(stream, objStudent);
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            
            
          return  js= sr.ReadToEnd();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
           // GoogleServiceAccount.GetToken();
           // var token = GoogleServiceAccount.GetAccessTokenFromJSONKey("Keys/viscousAuth.json", "https://www.googleapis.com/auth/userinfo.profile");
           // string ab = "acb";

           // string arr = new string(1);
            //Google.Apis.Oauth2.v2.Oauth2Service.Scope.UserinfoProfile and also 
            //Google.Apis.Oauth2.v2.Oauth2Service.Scope.UserinfoEmail 


         
        }

       

    }


    class DictionaryToDataTable
    {
        public static DataTable ConvertTo<T>(Dictionary<string, T> list, string dataTableName)
        {
           
            DataTable table = new DataTable("Customers");
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Phone", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("Category", typeof(string));
           
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (KeyValuePair<string, T> item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    if (prop.PropertyType.Name != "Dictionary`2")
                    {
                        if (prop.PropertyType.FullName == "System.String")
                        {
                            if (prop.GetValue(item.Value) == null)
                            {
                               // row[prop.Name] = prop.GetValue(item.Value);
                            }
                            else
                            {
                                row[prop.Name] = prop.GetValue(item.Value).ToString().Replace("'", "''");
                                if (prop.Name == "Category")
                                {
                                    row["ID"] = item.Key.ToString();
                                }
                            }
                        }
                        else
                        {
                          //  row[prop.Name] = prop.GetValue(item.Value);
                        }
                    }
                   
                table.Rows.Add(row);
            }
            return table;
        }

        //public static DataTable CreateTable1<T>(string dataTableName)
        //{
        //    Type entityType = typeof(T);
        //    DataTable table = new DataTable(dataTableName);
        //    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
        //    foreach (PropertyDescriptor prop in properties)
        //    {
        //        table.Columns.Add(prop.Name, prop.PropertyType);
        //    }
        //    return table;
        //}
    }


   
}