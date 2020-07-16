namespace FtpBackupProject
{
    partial class AddControllerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddControllerForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeView1 = new FtpBackupProject.CustomTreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.panelConn = new System.Windows.Forms.Panel();
            this.panelSet = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panelConn.SuspendLayout();
            this.panelSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.buttonConnect);
            this.groupBox1.Controls.Add(this.textBoxPassword);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxLogin);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxIP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(41, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 227);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FTP подключение";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(104, 31);
            this.textBoxName.MaxLength = 15;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(172, 22);
            this.textBoxName.TabIndex = 10;
            this.textBoxName.Text = "Мой контроллер";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Имя сервера:";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(104, 180);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(156, 30);
            this.buttonConnect.TabIndex = 8;
            this.buttonConnect.Text = "Подключиться";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(104, 143);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(172, 22);
            this.textBoxPassword.TabIndex = 7;
            this.textBoxPassword.Text = "service";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Пароль:";
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Location = new System.Drawing.Point(104, 115);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(172, 22);
            this.textBoxLogin.TabIndex = 5;
            this.textBoxLogin.Text = "plclogs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Логин:";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(104, 87);
            this.textBoxPort.MaxLength = 5;
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(172, 22);
            this.textBoxPort.TabIndex = 3;
            this.textBoxPort.Text = "21";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Порт:";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(104, 59);
            this.textBoxIP.MaxLength = 15;
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(172, 22);
            this.textBoxIP.TabIndex = 1;
            this.textBoxIP.Text = "62.213.11.155";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 344);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(920, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(118, 17);
            this.statusLabel.Text = "toolStripStatusLabel1";
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(3, 46);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(365, 268);
            this.treeView1.TabIndex = 2;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            this.imageList1.Images.SetKeyName(1, "file.png");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(51, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(273, 32);
            this.label6.TabIndex = 3;
            this.label6.Text = "Выберите конкретные файлы или папки,\r\nкопии которых требуется создавать:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelConn
            // 
            this.panelConn.Controls.Add(this.groupBox1);
            this.panelConn.Location = new System.Drawing.Point(228, 32);
            this.panelConn.Name = "panelConn";
            this.panelConn.Size = new System.Drawing.Size(473, 270);
            this.panelConn.TabIndex = 4;
            // 
            // panelSet
            // 
            this.panelSet.Controls.Add(this.listBox1);
            this.panelSet.Controls.Add(this.label7);
            this.panelSet.Controls.Add(this.label6);
            this.panelSet.Controls.Add(this.treeView1);
            this.panelSet.Location = new System.Drawing.Point(22, 12);
            this.panelSet.Name = "panelSet";
            this.panelSet.Size = new System.Drawing.Size(865, 329);
            this.panelSet.TabIndex = 5;
            this.panelSet.Visible = false;
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 18;
            this.listBox1.Location = new System.Drawing.Point(390, 46);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(472, 112);
            this.listBox1.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(502, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(225, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "Выбранные файлы и директории:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AddControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 366);
            this.Controls.Add(this.panelSet);
            this.Controls.Add(this.panelConn);
            this.Controls.Add(this.statusStrip1);
            this.Name = "AddControllerForm";
            this.Text = "AddControllerForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelConn.ResumeLayout(false);
            this.panelSet.ResumeLayout(false);
            this.panelSet.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private CustomTreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panelConn;
        private System.Windows.Forms.Panel panelSet;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label7;
    }
}