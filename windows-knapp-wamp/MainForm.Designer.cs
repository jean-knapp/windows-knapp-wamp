
namespace windows_knapp_wamp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mysqlGroup = new DevExpress.XtraEditors.GroupControl();
            this.stopMySqlButton = new DevExpress.XtraEditors.SimpleButton();
            this.startMySqlButton = new DevExpress.XtraEditors.SimpleButton();
            this.startApacheButton = new DevExpress.XtraEditors.SimpleButton();
            this.stopApacheButton = new DevExpress.XtraEditors.SimpleButton();
            this.apacheGroup = new DevExpress.XtraEditors.GroupControl();
            this.textEdit = new DevExpress.XtraEditors.TextEdit();
            this.browseButton = new DevExpress.XtraEditors.SimpleButton();
            this.folderBrowserDialog = new DevExpress.XtraEditors.XtraFolderBrowserDialog(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mysqlGroup)).BeginInit();
            this.mysqlGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apacheGroup)).BeginInit();
            this.apacheGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // mysqlGroup
            // 
            this.mysqlGroup.Controls.Add(this.stopMySqlButton);
            this.mysqlGroup.Controls.Add(this.startMySqlButton);
            this.mysqlGroup.Location = new System.Drawing.Point(183, 12);
            this.mysqlGroup.Name = "mysqlGroup";
            this.mysqlGroup.Size = new System.Drawing.Size(165, 55);
            this.mysqlGroup.TabIndex = 2;
            this.mysqlGroup.Text = "MySQL";
            // 
            // stopMySqlButton
            // 
            this.stopMySqlButton.Location = new System.Drawing.Point(83, 26);
            this.stopMySqlButton.Name = "stopMySqlButton";
            this.stopMySqlButton.Size = new System.Drawing.Size(75, 23);
            this.stopMySqlButton.TabIndex = 2;
            this.stopMySqlButton.Text = "Stop";
            this.stopMySqlButton.Click += new System.EventHandler(this.stopMySqlButton_Click);
            // 
            // startMySqlButton
            // 
            this.startMySqlButton.Location = new System.Drawing.Point(5, 26);
            this.startMySqlButton.Name = "startMySqlButton";
            this.startMySqlButton.Size = new System.Drawing.Size(75, 23);
            this.startMySqlButton.TabIndex = 0;
            this.startMySqlButton.Text = "Start";
            this.startMySqlButton.Click += new System.EventHandler(this.startMySqlButton_Click);
            // 
            // startApacheButton
            // 
            this.startApacheButton.Location = new System.Drawing.Point(5, 26);
            this.startApacheButton.Name = "startApacheButton";
            this.startApacheButton.Size = new System.Drawing.Size(75, 23);
            this.startApacheButton.TabIndex = 0;
            this.startApacheButton.Text = "Start";
            this.startApacheButton.Click += new System.EventHandler(this.startApacheButton_Click);
            // 
            // stopApacheButton
            // 
            this.stopApacheButton.Location = new System.Drawing.Point(86, 26);
            this.stopApacheButton.Name = "stopApacheButton";
            this.stopApacheButton.Size = new System.Drawing.Size(75, 23);
            this.stopApacheButton.TabIndex = 2;
            this.stopApacheButton.Text = "Stop";
            this.stopApacheButton.Click += new System.EventHandler(this.stopApacheButton_Click);
            // 
            // apacheGroup
            // 
            this.apacheGroup.Controls.Add(this.stopApacheButton);
            this.apacheGroup.Controls.Add(this.startApacheButton);
            this.apacheGroup.Location = new System.Drawing.Point(12, 12);
            this.apacheGroup.Name = "apacheGroup";
            this.apacheGroup.Size = new System.Drawing.Size(165, 55);
            this.apacheGroup.TabIndex = 1;
            this.apacheGroup.Text = "Apache";
            // 
            // textEdit
            // 
            this.textEdit.Location = new System.Drawing.Point(12, 73);
            this.textEdit.Name = "textEdit";
            this.textEdit.Properties.ReadOnly = true;
            this.textEdit.Size = new System.Drawing.Size(262, 20);
            this.textEdit.TabIndex = 3;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(273, 73);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 20);
            this.browseButton.TabIndex = 4;
            this.browseButton.Text = "Browse";
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.DialogStyle = DevExpress.Utils.CommonDialogs.FolderBrowserDialogStyle.Wide;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 106);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.textEdit);
            this.Controls.Add(this.mysqlGroup);
            this.Controls.Add(this.apacheGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("MainForm.IconOptions.Image")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Knapp WAMP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.mysqlGroup)).EndInit();
            this.mysqlGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.apacheGroup)).EndInit();
            this.apacheGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl mysqlGroup;
        private DevExpress.XtraEditors.SimpleButton stopMySqlButton;
        private DevExpress.XtraEditors.SimpleButton startMySqlButton;
        private DevExpress.XtraEditors.SimpleButton startApacheButton;
        private DevExpress.XtraEditors.SimpleButton stopApacheButton;
        private DevExpress.XtraEditors.GroupControl apacheGroup;
        private DevExpress.XtraEditors.TextEdit textEdit;
        private DevExpress.XtraEditors.SimpleButton browseButton;
        private DevExpress.XtraEditors.XtraFolderBrowserDialog folderBrowserDialog;
    }
}

