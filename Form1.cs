﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Jpeg;

namespace LongLapseOrganizer
{
    public partial class Form1 : Form
    {
        private const int FILE_MAGIC_NUMBER = 0x12658764;
        private string[] localFiles;
        private List<ImageRecord> mImageRecordList;
        private Dictionary<DateTime, int> mImageRecordsByDay;

        public Form1()
        {
            InitializeComponent();
            localFiles = new string[0];
            mImageRecordList = new List<ImageRecord>();
            mImageRecordsByDay = new Dictionary<DateTime, int>();
        }

        public void logMessage(String message)
        {
            richTextBoxLog.Text = message + "\r\n" + richTextBoxLog.Text;
        }

        public void updateImageRecordStatus()
        {
            if (mImageRecordList.Count > 0)
            {
                labelRecordStatus.Text = mImageRecordList.Count.ToString() + " records loaded";

                // Update map
                mImageRecordsByDay.Clear();
                foreach( ImageRecord ir in mImageRecordList)
                {
                    if(mImageRecordsByDay.ContainsKey(ir.CaptureTime.Date))
                    {
                        mImageRecordsByDay[ir.CaptureTime.Date]++;
                    }
                    else
                    {
                        mImageRecordsByDay.Add(ir.CaptureTime.Date, 1);
                    }
                }
                listView1.Items.Clear();
                foreach(KeyValuePair<DateTime, int> entry in mImageRecordsByDay)
                {
                    string[] newListStrings = new string[2];
                    newListStrings[0] = entry.Key.ToShortDateString();
                    newListStrings[1] = entry.Value.ToString();
                    listView1.Items.Add(new ListViewItem(newListStrings));
                }
            }
            else
            {
                labelRecordStatus.Text = "No images loaded";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                localFiles = System.IO.Directory.GetFiles(textBoxInputFolder.Text, "*.NEF");
                logMessage(localFiles.Length + " .NEF files found");
                buttonReadExif.Enabled = true;
                buttonSaveFilelist.Enabled = true;
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
            mImageRecordList = new List<ImageRecord>(localFiles.Length);
            foreach(String file in localFiles)
            {
                ImageRecord imageRecord = new ImageRecord();
                imageRecord.FileName = file;
                var directories = ImageMetadataReader.ReadMetadata(file);
                imageRecord.CaptureTime = directories[3].GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);
                mImageRecordList.Add(imageRecord);
                counter++;
            }
            mImageRecordList.Sort();
            updateImageRecordStatus();
            logMessage("Done processing");
        }

        private void btnSaveList_Click(object sender, EventArgs e)
        {
            int outImageIndex = 1;
            String outFolder = textBoxOutputFolder.Text;
            if (outFolder != null && System.IO.Directory.Exists(outFolder))
            {
                for (int i = 0; i < mImageRecordList.Count; i += (int)numericSkipNthFile.Value)
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C mklink \"" + outFolder + "\\" + textBoxOutputFileName.Text + outImageIndex.ToString("D4") + ".NEF\" \"" + mImageRecordList[i].FileName + "\"";
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

        private void buttonSaveFilelist_Click(object sender, EventArgs e)
        {
            int totalNumberOfBytes = 0;
            foreach(ImageRecord ir in mImageRecordList)
            {
                totalNumberOfBytes += ir.getByteLength();
            }

            // Creating uncompressed buffer
            logMessage("Compressing " + totalNumberOfBytes.ToString() + " bytes...");
            int index = 0;
            int irLength;
            byte[] uncompressedArray = new byte[totalNumberOfBytes];
            foreach (ImageRecord ir in mImageRecordList)
            {
                irLength = ir.getByteLength();
                ir.getBytes().CopyTo(uncompressedArray, index);
                index += irLength;
            }

            // Compressing
            byte[] compressedBytes;

            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    var fileInArchive = archive.CreateEntry("filelist_zipped", CompressionLevel.Optimal);
                    using (var entryStream = fileInArchive.Open())
                    using (var fileToCompressStream = new MemoryStream(uncompressedArray))
                    {
                        fileToCompressStream.CopyTo(entryStream);
                    }
                }
                compressedBytes = outStream.ToArray();
            }
            saveFileDialog1.ShowDialog();
            if(saveFileDialog1.FileName != null)
            {
                logMessage("Saving " + compressedBytes.Length + " bytes to " + saveFileDialog1.FileName);
                using (FileStream fs = File.OpenWrite(saveFileDialog1.FileName))
                {
                    fs.Write(BitConverter.GetBytes(FILE_MAGIC_NUMBER), 0, 4);
                    fs.Write(compressedBytes, 0, compressedBytes.Length);
                }
            }
        }

        private void buttonLoadFilelist_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if(openFileDialog1.FileName != null)
            {
                logMessage("Loading " + openFileDialog1.FileName);
                byte[] fileData = File.ReadAllBytes(openFileDialog1.FileName);

                if(BitConverter.ToInt32(fileData, 0) == FILE_MAGIC_NUMBER)
                {
                    byte[] uncompressedBytes = new byte[fileData.Length-4];
                    Array.Copy(fileData, 4, uncompressedBytes, 0, fileData.Length - 4);
                    using (var inStream = new MemoryStream(uncompressedBytes))
                    {
                        using (var archive = new ZipArchive(inStream, ZipArchiveMode.Read, true))
                        {
                            var fileInArchive = archive.GetEntry("filelist_zipped");
                            
                            Stream unzippedEntryStream = fileInArchive.Open();
                            mImageRecordList.Clear();
                            while(true)
                            {
                                ImageRecord newRecord = ImageRecord.ImageRecordFromStream(unzippedEntryStream);
                                if (newRecord == null) break;
                                mImageRecordList.Add(newRecord);
                            }
                        }
                    }
                    logMessage(mImageRecordList.Count.ToString() + " records loaded.");
                    updateImageRecordStatus();
                }
            }
        }
    }

    public class ImageRecord : IComparable<ImageRecord>
    {
        private String mFileName;
        private DateTime mDateTime;
        private bool mActive;

        public ImageRecord()
        {
            mActive = true;
        }

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

        public bool Active
        {
            get { return mActive; }
            set { mActive = value; }
        }

        // File related
        public int getByteLength()
        {
            return Encoding.ASCII.GetBytes(mFileName).Length + 2 // FileName
                   + 8                  // DateTime
                   + 1;                 // bool Active
        }

        public byte [] getBytes()
        {
            int arrayLength = getByteLength();
            byte[] returnArray = new byte[arrayLength];
            returnArray[0] = mActive ? (byte)1 : (byte)0;
            BitConverter.GetBytes(mDateTime.Ticks).CopyTo(returnArray, 1);
            BitConverter.GetBytes((UInt16)(mFileName.Length)).CopyTo(returnArray, 9);
            Encoding.ASCII.GetBytes(mFileName).CopyTo(returnArray, 11);
            return returnArray;
        }

        // Sorting
        public int CompareTo(ImageRecord other)
        {
            return mDateTime.CompareTo(other.CaptureTime);
        }

        static public ImageRecord ImageRecordFromStream(Stream inputStream)
        {
            ImageRecord newImageRecord = new ImageRecord();
            byte[] active = new byte[1];
            byte[] dateTime = new byte[8];
            byte[] nameLength = new byte[2];
            if(inputStream.Read(active, 0, 1) == 0) return null;
            if(inputStream.Read(dateTime, 0, 8) == 0) return null;
            if(inputStream.Read(nameLength, 0, 2) == 0) return null;
            byte[] name = new byte[BitConverter.ToUInt16(nameLength, 0)];
            if(inputStream.Read(name, 0, BitConverter.ToUInt16(nameLength, 0)) == 0) return null;
            newImageRecord.Active = (active[0] != 0);
            newImageRecord.CaptureTime = DateTime.FromBinary(BitConverter.ToInt64(dateTime, 0));
            newImageRecord.FileName = System.Text.Encoding.Default.GetString(name);
            return newImageRecord;
        }
    }
}
