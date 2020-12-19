﻿namespace LongLapseOrganizer
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.listView2 = new System.Windows.Forms.ListView();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonReadExif = new System.Windows.Forms.Button();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSaveList = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.textBoxInputFolder = new System.Windows.Forms.TextBox();
            this.textBoxOutputFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonInputBrowse = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.numericSkipNthFile = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxOutputFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonLoadFilelist = new System.Windows.Forms.Button();
            this.buttonSaveFilelist = new System.Windows.Forms.Button();
            this.groupBoxPicControls = new System.Windows.Forms.GroupBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.buttonSaveIntByDay = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelRecordStatus = new System.Windows.Forms.Label();
            this.groupBoxDaySettings = new System.Windows.Forms.GroupBox();
            this.numericDayCfgEndMinute = new System.Windows.Forms.NumericUpDown();
            this.numericDayCfgEndHour = new System.Windows.Forms.NumericUpDown();
            this.numericDayCfgStartMinute = new System.Windows.Forms.NumericUpDown();
            this.numericDayCfgStartHour = new System.Windows.Forms.NumericUpDown();
            this.numericDayCfgIntSec = new System.Windows.Forms.NumericUpDown();
            this.checkBoxDayCfgActive = new System.Windows.Forms.CheckBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericSkipNthFile)).BeginInit();
            this.groupBoxPicControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBoxDaySettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericDayCfgEndMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDayCfgEndHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDayCfgStartMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDayCfgStartHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDayCfgIntSec)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(280, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(801, 593);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Date";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "NumPics";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "First pic time";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Last pic time";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Num Selected Images";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "NumImagesByHour";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 58);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Load input files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView2
            // 
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(12, 542);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(262, 106);
            this.listView2.TabIndex = 2;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // buttonReadExif
            // 
            this.buttonReadExif.Enabled = false;
            this.buttonReadExif.Location = new System.Drawing.Point(6, 87);
            this.buttonReadExif.Name = "buttonReadExif";
            this.buttonReadExif.Size = new System.Drawing.Size(125, 23);
            this.buttonReadExif.TabIndex = 3;
            this.buttonReadExif.Text = "Read Exif";
            this.buttonReadExif.UseVisualStyleBackColor = true;
            this.buttonReadExif.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSaveList
            // 
            this.btnSaveList.Location = new System.Drawing.Point(6, 58);
            this.btnSaveList.Name = "btnSaveList";
            this.btnSaveList.Size = new System.Drawing.Size(125, 23);
            this.btnSaveList.TabIndex = 5;
            this.btnSaveList.Text = "Save List";
            this.btnSaveList.UseVisualStyleBackColor = true;
            this.btnSaveList.Click += new System.EventHandler(this.btnSaveList_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(12, 654);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(1107, 99);
            this.richTextBoxLog.TabIndex = 6;
            this.richTextBoxLog.Text = "";
            // 
            // textBoxInputFolder
            // 
            this.textBoxInputFolder.Location = new System.Drawing.Point(6, 32);
            this.textBoxInputFolder.Name = "textBoxInputFolder";
            this.textBoxInputFolder.Size = new System.Drawing.Size(166, 20);
            this.textBoxInputFolder.TabIndex = 7;
            this.textBoxInputFolder.Text = "M:\\breen_tl_backup\\part2";
            // 
            // textBoxOutputFolder
            // 
            this.textBoxOutputFolder.Location = new System.Drawing.Point(6, 100);
            this.textBoxOutputFolder.Name = "textBoxOutputFolder";
            this.textBoxOutputFolder.Size = new System.Drawing.Size(166, 20);
            this.textBoxOutputFolder.TabIndex = 8;
            this.textBoxOutputFolder.Text = "M:\\breen_tl_out_test";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Input folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Output folder";
            // 
            // buttonInputBrowse
            // 
            this.buttonInputBrowse.Location = new System.Drawing.Point(176, 30);
            this.buttonInputBrowse.Name = "buttonInputBrowse";
            this.buttonInputBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonInputBrowse.TabIndex = 11;
            this.buttonInputBrowse.Text = "Browse";
            this.buttonInputBrowse.UseVisualStyleBackColor = true;
            this.buttonInputBrowse.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(178, 98);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = "Browse";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // numericSkipNthFile
            // 
            this.numericSkipNthFile.Location = new System.Drawing.Point(6, 32);
            this.numericSkipNthFile.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericSkipNthFile.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericSkipNthFile.Name = "numericSkipNthFile";
            this.numericSkipNthFile.Size = new System.Drawing.Size(77, 20);
            this.numericSkipNthFile.TabIndex = 13;
            this.numericSkipNthFile.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Skip Nth frame";
            // 
            // textBoxOutputFileName
            // 
            this.textBoxOutputFileName.Location = new System.Drawing.Point(89, 32);
            this.textBoxOutputFileName.Name = "textBoxOutputFileName";
            this.textBoxOutputFileName.Size = new System.Drawing.Size(162, 20);
            this.textBoxOutputFileName.TabIndex = 15;
            this.textBoxOutputFileName.Text = "breen_int_test_";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Output file name";
            // 
            // buttonLoadFilelist
            // 
            this.buttonLoadFilelist.Location = new System.Drawing.Point(137, 58);
            this.buttonLoadFilelist.Name = "buttonLoadFilelist";
            this.buttonLoadFilelist.Size = new System.Drawing.Size(114, 23);
            this.buttonLoadFilelist.TabIndex = 17;
            this.buttonLoadFilelist.Text = "Load filelist";
            this.buttonLoadFilelist.UseVisualStyleBackColor = true;
            this.buttonLoadFilelist.Click += new System.EventHandler(this.buttonLoadFilelist_Click);
            // 
            // buttonSaveFilelist
            // 
            this.buttonSaveFilelist.Enabled = false;
            this.buttonSaveFilelist.Location = new System.Drawing.Point(137, 87);
            this.buttonSaveFilelist.Name = "buttonSaveFilelist";
            this.buttonSaveFilelist.Size = new System.Drawing.Size(114, 23);
            this.buttonSaveFilelist.TabIndex = 18;
            this.buttonSaveFilelist.Text = "Save Filelist";
            this.buttonSaveFilelist.UseVisualStyleBackColor = true;
            this.buttonSaveFilelist.Click += new System.EventHandler(this.buttonSaveFilelist_Click);
            // 
            // groupBoxPicControls
            // 
            this.groupBoxPicControls.Controls.Add(this.numericUpDown1);
            this.groupBoxPicControls.Controls.Add(this.buttonSaveIntByDay);
            this.groupBoxPicControls.Controls.Add(this.label4);
            this.groupBoxPicControls.Controls.Add(this.numericSkipNthFile);
            this.groupBoxPicControls.Controls.Add(this.label3);
            this.groupBoxPicControls.Controls.Add(this.button4);
            this.groupBoxPicControls.Controls.Add(this.textBoxOutputFileName);
            this.groupBoxPicControls.Controls.Add(this.btnSaveList);
            this.groupBoxPicControls.Controls.Add(this.label2);
            this.groupBoxPicControls.Controls.Add(this.textBoxOutputFolder);
            this.groupBoxPicControls.Enabled = false;
            this.groupBoxPicControls.Location = new System.Drawing.Point(12, 170);
            this.groupBoxPicControls.Name = "groupBoxPicControls";
            this.groupBoxPicControls.Size = new System.Drawing.Size(259, 172);
            this.groupBoxPicControls.TabIndex = 19;
            this.groupBoxPicControls.TabStop = false;
            this.groupBoxPicControls.Text = "Picture controls";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(178, 139);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(73, 20);
            this.numericUpDown1.TabIndex = 18;
            this.numericUpDown1.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // buttonSaveIntByDay
            // 
            this.buttonSaveIntByDay.Location = new System.Drawing.Point(6, 136);
            this.buttonSaveIntByDay.Name = "buttonSaveIntByDay";
            this.buttonSaveIntByDay.Size = new System.Drawing.Size(166, 23);
            this.buttonSaveIntByDay.TabIndex = 17;
            this.buttonSaveIntByDay.Text = "Save Timelapse by day";
            this.buttonSaveIntByDay.UseVisualStyleBackColor = true;
            this.buttonSaveIntByDay.Click += new System.EventHandler(this.buttonSaveIntervalByDay_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBoxInputFolder);
            this.groupBox2.Controls.Add(this.buttonSaveFilelist);
            this.groupBox2.Controls.Add(this.buttonInputBrowse);
            this.groupBox2.Controls.Add(this.buttonLoadFilelist);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.buttonReadExif);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 152);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Project files";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "llo";
            this.saveFileDialog1.InitialDirectory = "M:\\breen_tl_backup\\part2";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "LongLapse file list|*.llo";
            this.openFileDialog1.InitialDirectory = "M:\\breen_tl_backup\\part2";
            // 
            // labelRecordStatus
            // 
            this.labelRecordStatus.AutoSize = true;
            this.labelRecordStatus.Location = new System.Drawing.Point(277, 9);
            this.labelRecordStatus.Name = "labelRecordStatus";
            this.labelRecordStatus.Size = new System.Drawing.Size(97, 13);
            this.labelRecordStatus.TabIndex = 21;
            this.labelRecordStatus.Text = "No Images Loaded";
            // 
            // groupBoxDaySettings
            // 
            this.groupBoxDaySettings.Controls.Add(this.numericDayCfgEndMinute);
            this.groupBoxDaySettings.Controls.Add(this.numericDayCfgEndHour);
            this.groupBoxDaySettings.Controls.Add(this.numericDayCfgStartMinute);
            this.groupBoxDaySettings.Controls.Add(this.numericDayCfgStartHour);
            this.groupBoxDaySettings.Controls.Add(this.numericDayCfgIntSec);
            this.groupBoxDaySettings.Controls.Add(this.checkBoxDayCfgActive);
            this.groupBoxDaySettings.Location = new System.Drawing.Point(12, 348);
            this.groupBoxDaySettings.Name = "groupBoxDaySettings";
            this.groupBoxDaySettings.Size = new System.Drawing.Size(259, 188);
            this.groupBoxDaySettings.TabIndex = 22;
            this.groupBoxDaySettings.TabStop = false;
            this.groupBoxDaySettings.Text = "Day Settings";
            // 
            // numericDayCfgEndMinute
            // 
            this.numericDayCfgEndMinute.Location = new System.Drawing.Point(166, 92);
            this.numericDayCfgEndMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericDayCfgEndMinute.Name = "numericDayCfgEndMinute";
            this.numericDayCfgEndMinute.Size = new System.Drawing.Size(38, 20);
            this.numericDayCfgEndMinute.TabIndex = 5;
            this.numericDayCfgEndMinute.ValueChanged += new System.EventHandler(this.numericDayCfgEndHour_ValueChanged);
            // 
            // numericDayCfgEndHour
            // 
            this.numericDayCfgEndHour.Location = new System.Drawing.Point(122, 92);
            this.numericDayCfgEndHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericDayCfgEndHour.Name = "numericDayCfgEndHour";
            this.numericDayCfgEndHour.Size = new System.Drawing.Size(38, 20);
            this.numericDayCfgEndHour.TabIndex = 4;
            this.numericDayCfgEndHour.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.numericDayCfgEndHour.ValueChanged += new System.EventHandler(this.numericDayCfgEndHour_ValueChanged);
            // 
            // numericDayCfgStartMinute
            // 
            this.numericDayCfgStartMinute.Location = new System.Drawing.Point(50, 92);
            this.numericDayCfgStartMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericDayCfgStartMinute.Name = "numericDayCfgStartMinute";
            this.numericDayCfgStartMinute.Size = new System.Drawing.Size(38, 20);
            this.numericDayCfgStartMinute.TabIndex = 3;
            this.numericDayCfgStartMinute.ValueChanged += new System.EventHandler(this.numericDayCfgStartHour_ValueChanged);
            // 
            // numericDayCfgStartHour
            // 
            this.numericDayCfgStartHour.Location = new System.Drawing.Point(6, 92);
            this.numericDayCfgStartHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericDayCfgStartHour.Name = "numericDayCfgStartHour";
            this.numericDayCfgStartHour.Size = new System.Drawing.Size(38, 20);
            this.numericDayCfgStartHour.TabIndex = 2;
            this.numericDayCfgStartHour.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericDayCfgStartHour.ValueChanged += new System.EventHandler(this.numericDayCfgStartHour_ValueChanged);
            // 
            // numericDayCfgIntSec
            // 
            this.numericDayCfgIntSec.Location = new System.Drawing.Point(6, 51);
            this.numericDayCfgIntSec.Name = "numericDayCfgIntSec";
            this.numericDayCfgIntSec.Size = new System.Drawing.Size(93, 20);
            this.numericDayCfgIntSec.TabIndex = 1;
            this.numericDayCfgIntSec.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericDayCfgIntSec.ValueChanged += new System.EventHandler(this.numericDayCfgIntSec_ValueChanged);
            // 
            // checkBoxDayCfgActive
            // 
            this.checkBoxDayCfgActive.AutoSize = true;
            this.checkBoxDayCfgActive.Location = new System.Drawing.Point(6, 19);
            this.checkBoxDayCfgActive.Name = "checkBoxDayCfgActive";
            this.checkBoxDayCfgActive.Size = new System.Drawing.Size(56, 17);
            this.checkBoxDayCfgActive.TabIndex = 0;
            this.checkBoxDayCfgActive.Text = "Active";
            this.checkBoxDayCfgActive.UseVisualStyleBackColor = true;
            this.checkBoxDayCfgActive.CheckedChanged += new System.EventHandler(this.checkBoxDayCfgActive_CheckedChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(281, 624);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(93, 23);
            this.buttonRefresh.TabIndex = 23;
            this.buttonRefresh.Text = "buttonRefresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 765);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.groupBoxDaySettings);
            this.Controls.Add(this.labelRecordStatus);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxPicControls);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numericSkipNthFile)).EndInit();
            this.groupBoxPicControls.ResumeLayout(false);
            this.groupBoxPicControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxDaySettings.ResumeLayout(false);
            this.groupBoxDaySettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericDayCfgEndMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDayCfgEndHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDayCfgStartMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDayCfgStartHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDayCfgIntSec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button buttonReadExif;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.Button btnSaveList;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.TextBox textBoxInputFolder;
        private System.Windows.Forms.TextBox textBoxOutputFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonInputBrowse;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.NumericUpDown numericSkipNthFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxOutputFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonLoadFilelist;
        private System.Windows.Forms.Button buttonSaveFilelist;
        private System.Windows.Forms.GroupBox groupBoxPicControls;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label labelRecordStatus;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button buttonSaveIntByDay;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.GroupBox groupBoxDaySettings;
        private System.Windows.Forms.NumericUpDown numericDayCfgEndMinute;
        private System.Windows.Forms.NumericUpDown numericDayCfgEndHour;
        private System.Windows.Forms.NumericUpDown numericDayCfgStartMinute;
        private System.Windows.Forms.NumericUpDown numericDayCfgStartHour;
        private System.Windows.Forms.NumericUpDown numericDayCfgIntSec;
        private System.Windows.Forms.CheckBox checkBoxDayCfgActive;
        private System.Windows.Forms.Button buttonRefresh;
    }
}

