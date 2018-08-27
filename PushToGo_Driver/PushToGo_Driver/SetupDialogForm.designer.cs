namespace ASCOM.PushToGo
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxArea = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxAperture = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxElevation = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxLatitude = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxTemp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLongitude = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxGuideSpeed = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkRefraction = new System.Windows.Forms.CheckBox();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.buttonSyncTime = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(426, 172);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(426, 202);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.PushToGo.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(437, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Serial Port";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxArea);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxAperture);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxFL);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(10, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 101);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optics";
            // 
            // textBoxArea
            // 
            this.textBoxArea.Location = new System.Drawing.Point(99, 70);
            this.textBoxArea.Name = "textBoxArea";
            this.textBoxArea.Size = new System.Drawing.Size(73, 20);
            this.textBoxArea.TabIndex = 15;
            this.textBoxArea.TextChanged += new System.EventHandler(this.textBoxArea_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Apert. Area (m^2)";
            // 
            // textBoxAperture
            // 
            this.textBoxAperture.Location = new System.Drawing.Point(99, 42);
            this.textBoxAperture.Name = "textBoxAperture";
            this.textBoxAperture.Size = new System.Drawing.Size(73, 20);
            this.textBoxAperture.TabIndex = 13;
            this.textBoxAperture.TextChanged += new System.EventHandler(this.textBoxAperture_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Aperture (m)";
            // 
            // textBoxFL
            // 
            this.textBoxFL.Location = new System.Drawing.Point(99, 16);
            this.textBoxFL.Name = "textBoxFL";
            this.textBoxFL.Size = new System.Drawing.Size(73, 20);
            this.textBoxFL.TabIndex = 11;
            this.textBoxFL.TextChanged += new System.EventHandler(this.textBoxFL_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Focal Length (m)";
            // 
            // textBoxElevation
            // 
            this.textBoxElevation.Location = new System.Drawing.Point(107, 15);
            this.textBoxElevation.Name = "textBoxElevation";
            this.textBoxElevation.Size = new System.Drawing.Size(73, 20);
            this.textBoxElevation.TabIndex = 17;
            this.textBoxElevation.TextChanged += new System.EventHandler(this.textBoxElevation_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Elevation (m)";
            // 
            // textBoxLatitude
            // 
            this.textBoxLatitude.Location = new System.Drawing.Point(107, 66);
            this.textBoxLatitude.Name = "textBoxLatitude";
            this.textBoxLatitude.ReadOnly = true;
            this.textBoxLatitude.Size = new System.Drawing.Size(73, 20);
            this.textBoxLatitude.TabIndex = 19;
            this.textBoxLatitude.LostFocus += new System.EventHandler(this.textBoxLatitude_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Latitude";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxTemp);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBoxLongitude);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBoxLatitude);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBoxElevation);
            this.groupBox2.Location = new System.Drawing.Point(206, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 120);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Site Setting";
            // 
            // textBoxTemp
            // 
            this.textBoxTemp.Location = new System.Drawing.Point(107, 40);
            this.textBoxTemp.Name = "textBoxTemp";
            this.textBoxTemp.Size = new System.Drawing.Size(73, 20);
            this.textBoxTemp.TabIndex = 23;
            this.textBoxTemp.TextChanged += new System.EventHandler(this.textBoxTemp_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Temperature";
            // 
            // textBoxLongitude
            // 
            this.textBoxLongitude.Location = new System.Drawing.Point(107, 92);
            this.textBoxLongitude.Name = "textBoxLongitude";
            this.textBoxLongitude.ReadOnly = true;
            this.textBoxLongitude.Size = new System.Drawing.Size(73, 20);
            this.textBoxLongitude.TabIndex = 21;
            this.textBoxLongitude.LostFocus += new System.EventHandler(this.textBoxLongitude_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Longitude";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxGuideSpeed);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(206, 138);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 79);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Mount Settings";
            // 
            // textBoxGuideSpeed
            // 
            this.textBoxGuideSpeed.Location = new System.Drawing.Point(107, 23);
            this.textBoxGuideSpeed.Name = "textBoxGuideSpeed";
            this.textBoxGuideSpeed.ReadOnly = true;
            this.textBoxGuideSpeed.Size = new System.Drawing.Size(73, 20);
            this.textBoxGuideSpeed.TabIndex = 25;
            this.textBoxGuideSpeed.LostFocus += new System.EventHandler(this.textBoxGuideSpeed_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Guide Speed      x";
            // 
            // chkRefraction
            // 
            this.chkRefraction.AutoSize = true;
            this.chkRefraction.Checked = global::ASCOM.PushToGo.Properties.Settings.Default.refraction;
            this.chkRefraction.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.PushToGo.Properties.Settings.Default, "refraction", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkRefraction.Location = new System.Drawing.Point(12, 55);
            this.chkRefraction.Name = "chkRefraction";
            this.chkRefraction.Size = new System.Drawing.Size(75, 17);
            this.chkRefraction.TabIndex = 8;
            this.chkRefraction.Text = "Refraction";
            this.chkRefraction.UseVisualStyleBackColor = true;
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.PushToGo.Properties.Settings.Default, "comPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboBoxComPort.DisplayMember = "COM1";
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(75, 12);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(90, 21);
            this.comboBoxComPort.TabIndex = 7;
            this.comboBoxComPort.Text = global::ASCOM.PushToGo.Properties.Settings.Default.comPort;
            this.comboBoxComPort.ValueMember = "COM1";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Checked = global::ASCOM.PushToGo.Properties.Settings.Default.traceEnabled;
            this.chkTrace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrace.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.PushToGo.Properties.Settings.Default, "traceEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkTrace.Location = new System.Drawing.Point(12, 36);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // buttonSyncTime
            // 
            this.buttonSyncTime.Enabled = false;
            this.buttonSyncTime.Location = new System.Drawing.Point(107, 42);
            this.buttonSyncTime.Name = "buttonSyncTime";
            this.buttonSyncTime.Size = new System.Drawing.Size(75, 23);
            this.buttonSyncTime.TabIndex = 26;
            this.buttonSyncTime.Text = "Sync Time";
            this.buttonSyncTime.UseVisualStyleBackColor = true;
            this.buttonSyncTime.Click += new System.EventHandler(this.buttonSyncTime_Click);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 236);
            this.Controls.Add(this.buttonSyncTime);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkRefraction);
            this.Controls.Add(this.comboBoxComPort);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PushToGo Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.CheckBox chkRefraction;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxFL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxAperture;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxArea;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxElevation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxLatitude;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxLongitude;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxTemp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxGuideSpeed;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonSyncTime;
    }
}