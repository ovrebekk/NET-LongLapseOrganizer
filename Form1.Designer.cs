namespace LongLapseOrganizer
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelRecordStatus = new System.Windows.Forms.Label();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.numericSkipNthFile)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(362, 42);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(340, 222);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
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
            this.listView2.Location = new System.Drawing.Point(362, 270);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(340, 106);
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
            this.richTextBoxLog.Location = new System.Drawing.Point(12, 397);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numericSkipNthFile);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.textBoxOutputFileName);
            this.groupBox1.Controls.Add(this.btnSaveList);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxOutputFolder);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(12, 170);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 185);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Picture controls";
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
            this.labelRecordStatus.Location = new System.Drawing.Point(359, 26);
            this.labelRecordStatus.Name = "labelRecordStatus";
            this.labelRecordStatus.Size = new System.Drawing.Size(97, 13);
            this.labelRecordStatus.TabIndex = 21;
            this.labelRecordStatus.Text = "No Images Loaded";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Date";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "NumPics";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 508);
            this.Controls.Add(this.labelRecordStatus);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numericSkipNthFile)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label labelRecordStatus;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}

