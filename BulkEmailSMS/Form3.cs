using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BulkEmailSMS
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {
            AdminSignUp f = new AdminSignUp();
            f.Show();
            this.Hide();
        }

        private void btnContacts_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            designbtns();
        }

        public void designbtns()
        {
            btnClose.TabStop = false;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnClose.FlatAppearance.MouseOverBackColor = Color.Transparent;

            btnContacts.TabStop = false;
            btnContacts.FlatStyle = FlatStyle.Flat;
            btnContacts.FlatAppearance.BorderSize = 0;
            btnContacts.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnContacts.FlatAppearance.MouseOverBackColor = Color.Transparent;

            btnSmpt.TabStop = false;
            btnSmpt.FlatStyle = FlatStyle.Flat;
            btnSmpt.FlatAppearance.BorderSize = 0;
            btnSmpt.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSmpt.FlatAppearance.MouseOverBackColor = Color.Transparent;

            btnSendEmail.TabStop = false;
            btnSendEmail.FlatStyle = FlatStyle.Flat;
            btnSendEmail.FlatAppearance.BorderSize = 0;
            btnSendEmail.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSendEmail.FlatAppearance.MouseOverBackColor = Color.Transparent;

            
        }

        private void btnSendSMS_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            SMS sms = new SMS();
            sms.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SMS sms = new SMS();
            sms.Show();
            this.Hide();
        }
    }
}
