namespace REST.Service
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label connectionStringLabel;
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonStartWebService = new System.Windows.Forms.Button();
            this.textRecordsPerPage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textListenerPrefix = new System.Windows.Forms.TextBox();
            this.textConnectionString = new System.Windows.Forms.TextBox();
            this.sQLWebServiceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            connectionStringLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sQLWebServiceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // connectionStringLabel
            // 
            connectionStringLabel.AutoSize = true;
            connectionStringLabel.Location = new System.Drawing.Point(12, 32);
            connectionStringLabel.Name = "connectionStringLabel";
            connectionStringLabel.Size = new System.Drawing.Size(140, 20);
            connectionStringLabel.TabIndex = 9;
            connectionStringLabel.Text = "Connection String:";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(915, 20);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(172, 45);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonStartWebService
            // 
            this.buttonStartWebService.Location = new System.Drawing.Point(915, 75);
            this.buttonStartWebService.Name = "buttonStartWebService";
            this.buttonStartWebService.Size = new System.Drawing.Size(172, 45);
            this.buttonStartWebService.TabIndex = 1;
            this.buttonStartWebService.Text = "Start Web Service";
            this.buttonStartWebService.UseVisualStyleBackColor = true;
            this.buttonStartWebService.Click += new System.EventHandler(this.buttonStartWebService_Click);
            // 
            // textRecordsPerPage
            // 
            this.textRecordsPerPage.Location = new System.Drawing.Point(842, 84);
            this.textRecordsPerPage.Name = "textRecordsPerPage";
            this.textRecordsPerPage.Size = new System.Drawing.Size(67, 26);
            this.textRecordsPerPage.TabIndex = 3;
            this.textRecordsPerPage.TextChanged += new System.EventHandler(this.textRecordsPerPage_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(696, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Records per page:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Listener prefix:";
            // 
            // textListenerPrefix
            // 
            this.textListenerPrefix.Location = new System.Drawing.Point(158, 84);
            this.textListenerPrefix.Name = "textListenerPrefix";
            this.textListenerPrefix.Size = new System.Drawing.Size(234, 26);
            this.textListenerPrefix.TabIndex = 7;
            this.textListenerPrefix.TextChanged += new System.EventHandler(this.textListenerPrefix_TextChanged);
            // 
            // textConnectionString
            // 
            this.textConnectionString.Location = new System.Drawing.Point(158, 29);
            this.textConnectionString.Name = "textConnectionString";
            this.textConnectionString.Size = new System.Drawing.Size(751, 26);
            this.textConnectionString.TabIndex = 10;
            this.textConnectionString.TextChanged += new System.EventHandler(this.connectionStringTextBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 383);
            this.Controls.Add(connectionStringLabel);
            this.Controls.Add(this.textConnectionString);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textListenerPrefix);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textRecordsPerPage);
            this.Controls.Add(this.buttonStartWebService);
            this.Controls.Add(this.buttonConnect);
            this.Name = "Form1";
            this.Text = "RESTful Web Service";
            ((System.ComponentModel.ISupportInitialize)(this.sQLWebServiceBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonStartWebService;
        private System.Windows.Forms.TextBox textRecordsPerPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textListenerPrefix;
        private System.Windows.Forms.BindingSource sQLWebServiceBindingSource;
        private System.Windows.Forms.TextBox textConnectionString;
    }
}

