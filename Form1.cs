using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Jpeg;

namespace LongLapseOrganizer
{
    public partial class Form1 : Form
    {
        private string[] localFiles;
        private List<ImageRecord> imageRecordList;

        public Form1()
        {
            InitializeComponent();
            localFiles = new string[0];
            imageRecordList = new List<ImageRecord>();
        }

        public void logMessage(String message)
        {
            richTextBoxLog.Text = message + "\r\n" + richTextBoxLog.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                localFiles = System.IO.Directory.GetFiles(textBoxInputFolder.Text, "*.NEF");
                logMessage(localFiles.Length + " .NEF files found");
            }
            catch(Exception exception)
            {
                logMessage(exception.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Exif button loaded
            logMessage("Reading EXIF data: " + localFiles[0]);
            int counter = 0;
            imageRecordList = new List<ImageRecord>(localFiles.Length);
            foreach(String file in localFiles)
            {
                ImageRecord imageRecord = new ImageRecord();
                imageRecord.FileName = file;
                var directories = ImageMetadataReader.ReadMetadata(file);
                imageRecord.CaptureTime = directories[3].GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);
                imageRecordList.Add(imageRecord);
                counter++;
            }
            imageRecordList.Sort();
            logMessage("Done processing");
        }

        private void btnSaveList_Click(object sender, EventArgs e)
        {
            int outImageIndex = 1;
            String outFolder = textBoxOutputFolder.Text;
            if (outFolder != null && System.IO.Directory.Exists(outFolder))
            {
                for (int i = 0; i < imageRecordList.Count; i += (int)numericSkipNthFile.Value)
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C mklink \"" + outFolder + "\\" + textBoxOutputFileName.Text + outImageIndex.ToString("D4") + ".NEF\" \"" + imageRecordList[i].FileName + "\"";
                    startInfo.Verb = "runas";
                    outImageIndex++;
                    process.StartInfo = startInfo;
                    process.Start();
                }
                logMessage(outImageIndex.ToString() + " symbolic links created");
            }
            else logMessage("Invalid out folder");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBoxInputFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            folderBrowserDialog2.ShowDialog();
            textBoxOutputFolder.Text = folderBrowserDialog2.SelectedPath;
        }
    }
    public class ImageRecord : IComparable<ImageRecord>
    {
        private String mFileName;
        private DateTime mDateTime;

        public DateTime CaptureTime
        {
            get { return mDateTime; }
            set { mDateTime = value; }
        }

        public String FileName
        {
            get { return mFileName; }
            set { mFileName = value; }
        }

        public int CompareTo(ImageRecord other)
        {
            return mDateTime.CompareTo(other.CaptureTime);
        }
    }
}
