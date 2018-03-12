namespace BulkEmailSMS
{
    partial class mainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnContacts = new System.Windows.Forms.Button();
            this.btnSendEmail = new System.Windows.Forms.Button();
            this.btnSmpt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::BulkEmailSMS.Properties.Resources.sms_icon_5476;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClose.Location = new System.Drawing.Point(194, 31);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(169, 132);
            this.btnClose.TabIndex = 3;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnContacts
            // 
            this.btnContacts.BackColor = System.Drawing.Color.Transparent;
            this.btnContacts.BackgroundImage = global::BulkEmailSMS.Properties.Resources.psd6EDD7DF4A2957FC595677275080822E080C194DDF3EAF689A_pimgpsh_fullsize_distr;
            this.btnContacts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnContacts.Location = new System.Drawing.Point(194, 170);
            this.btnContacts.Name = "btnContacts";
            this.btnContacts.Size = new System.Drawing.Size(169, 132);
            this.btnContacts.TabIndex = 2;
            this.btnContacts.UseVisualStyleBackColor = false;
            this.btnContacts.Click += new System.EventHandler(this.btnContacts_Click);
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.BackColor = System.Drawing.Color.Transparent;
            this.btnSendEmail.BackgroundImage = global::BulkEmailSMS.Properties.Resources.Communication_email_2_icon;
            this.btnSendEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSendEmail.Location = new System.Drawing.Point(19, 31);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(169, 132);
            this.btnSendEmail.TabIndex = 1;
            this.btnSendEmail.UseVisualStyleBackColor = false;
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            // 
            // btnSmpt
            // 
            this.btnSmpt.BackColor = System.Drawing.Color.Transparent;
            this.btnSmpt.BackgroundImage = global::BulkEmailSMS.Properties.Resources.Login_Key;
            this.btnSmpt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSmpt.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSmpt.Location = new System.Drawing.Point(19, 168);
            this.btnSmpt.Name = "btnSmpt";
            this.btnSmpt.Size = new System.Drawing.Size(169, 132);
            this.btnSmpt.TabIndex = 0;
            this.btnSmpt.UseVisualStyleBackColor = false;
            this.btnSmpt.Click += new System.EventHandler(this.btnSendSMS_Click);
            this.btnSmpt.MouseEnter += new System.EventHandler(this.btnSendSMS_MouseEnter);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BulkEmailSMS.Properties.Resources.dgvBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(386, 314);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnContacts);
            this.Controls.Add(this.btnSendEmail);
            this.Controls.Add(this.btnSmpt);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BulkEmailSMS";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSmpt;
        private System.Windows.Forms.Button btnSendEmail;
        private System.Windows.Forms.Button btnContacts;
        private System.Windows.Forms.Button btnClose;
    }
}