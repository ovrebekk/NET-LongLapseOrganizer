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
        private const int FILE_DAYCFG_MAGIC_NUMBER = 0x63B25FA3;
        private string[] localFiles;
        private string openImageFile = "";
        private List<ImageRecord> mImageRecordList;
        private Dictionary<DateTime, ImagesByDaySummary> mImageRecordsByDay;
        private ImagesByDaySummary mSelectedDay = null;

        public Form1()
        {
            InitializeComponent();
            localFiles = new string[0];
            mImageRecordList = new List<ImageRecord>();
            mImageRecordsByDay = new Dictionary<DateTime, ImagesByDaySummary>();
        }

        public void logMessage(String message)
        {
            richTextBoxLog.Text = message + "\r\n" + richTextBoxLog.Text;
        }

        public void createSymLink(string targetFile, string outputPath)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C mklink \"" + outputPath + "\" \"" + targetFile + "\"";
            startInfo.Verb = "runas";
            process.StartInfo = startInfo;
            process.Start();
        }

        public void updateImageRecordStatus(bool totalRefresh)
        {
            if (mImageRecordList.Count > 0)
            {
                int selectedImagesTotal = 0;

                labelRecordStatus.Text = mImageRecordList.Count.ToString() + " records loaded";

                // Update map
                if (totalRefresh)
                {
                    mImageRecordsByDay.Clear();
                    foreach (ImageRecord ir in mImageRecordList)
                    {
                        if (mImageRecordsByDay.ContainsKey(ir.CaptureTime.Date))
                        {
                            mImageRecordsByDay[ir.CaptureTime.Date].NumberOfPictures++;
                            mImageRecordsByDay[ir.CaptureTime.Date].registerDateTime(ir.CaptureTime);
                            mImageRecordsByDay[ir.CaptureTime.Date].addImage(ir);
                        }
                        else
                        {
                            ImagesByDaySummary summary = new ImagesByDaySummary(ir.CaptureTime);
                            summary.addImage(ir);
                            summary.Day = ir.CaptureTime.Date;
                            mImageRecordsByDay.Add(ir.CaptureTime.Date, summary);
                        }
                    }

                    groupBoxPicControls.Enabled = true;
                    groupBoxDaySettings.Enabled = true;
                }
                else
                {
                    foreach (KeyValuePair<DateTime, ImagesByDaySummary> entry in mImageRecordsByDay)
                    {
                        entry.Value.clearImages();
                    }
                    foreach (ImageRecord ir in mImageRecordList)
                    {
                        //mImageRecordsByDay[ir.CaptureTime.Date].NumberOfPictures++;
                        //mImageRecordsByDay[ir.CaptureTime.Date].registerDateTime(ir.CaptureTime);
                        mImageRecordsByDay[ir.CaptureTime.Date].addImage(ir);
                    }
                }
                listView1.Items.Clear();
                foreach (KeyValuePair<DateTime, ImagesByDaySummary> entry in mImageRecordsByDay)
                {
                    if (checkBoxShowInactive.Checked || entry.Value.CfgActive)
                    {
                        string[] newListStrings = new string[10];
                        newListStrings[0] = entry.Key.ToShortDateString();
                        newListStrings[1] = entry.Value.NumberOfPictures.ToString();
                        newListStrings[2] = entry.Value.FirstTime.ToShortTimeString();
                        newListStrings[3] = entry.Value.LastTime.ToShortTimeString();
                        newListStrings[4] = entry.Value.SelectedImages.Count.ToString();
                        newListStrings[5] = entry.Value.CfgActive ? "Yes" : "-";
                        newListStrings[6] = entry.Value.CfgIntervalSec.ToString();
                        newListStrings[7] = entry.Value.CfgStartTime.ToString();
                        newListStrings[8] = entry.Value.CfgEndTime.ToString();
                        newListStrings[9] = entry.Value.getNumImagesByHourString();
                        ListViewItem listViewItem = new ListViewItem(newListStrings);
                        listViewItem.Tag = entry.Value;
                        listView1.Items.Add(listViewItem);
                    }
                    if (entry.Value.CfgActive) selectedImagesTotal += entry.Value.SelectedImages.Count;
                }
                labelTotalSelectedFiles.Text = "Total selected files: " + selectedImagesTotal.ToString();
            }
            else
            {
                labelRecordStatus.Text = "No images loaded";
                groupBoxPicControls.Enabled = false;
                groupBoxDaySettings.Enabled = false;
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
            updateImageRecordStatus(true);
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
            if(openFileDialog1.FileName != null && openFileDialog1.FileName != "")
            {
                logMessage("Loading " + openFileDialog1.FileName);
                openImageFile = openFileDialog1.FileName;
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
                    
                    updateImageRecordStatus(true);
                    
                    // Populate combobox of day setting files based on files in the same directory
                    comboBox1.Items.Clear();
                    string[] daySettingFiles = System.IO.Directory.GetFiles(Path.GetDirectoryName(openFileDialog1.FileName), 
                                                                            "*.llods", SearchOption.AllDirectories);
                    foreach (String file in daySettingFiles)
                    {
                        comboBox1.Items.Add(Path.GetFileNameWithoutExtension(file));
                    }
                }
            }
        }

        private void buttonSaveIntervalByDay_Click(object sender, EventArgs e)
        {
            string standardFileName = "brtest_";
            if (textBoxOutputFolder.Text == null || textBoxOutputFolder.Text == "")
            {
                logMessage("Error: No output directory provided!");
            }
            else if(!System.IO.Directory.Exists(textBoxOutputFolder.Text))
            {
                logMessage("Error: Output directory doesn't exist!");
            }
            else
            {
                foreach (KeyValuePair<DateTime, ImagesByDaySummary> dayEntry in mImageRecordsByDay)
                {
                    if(dayEntry.Value.CfgActive)
                    {
                        string dayDirectory = textBoxOutputFolder.Text + "\\" + dayEntry.Key.ToString("yy_MM_dd");
                        System.IO.Directory.CreateDirectory(dayDirectory);
                        int imageIndex = 1;
                        foreach(ImageRecord selectedImage in dayEntry.Value.SelectedImages)
                        {
                            createSymLink(selectedImage.FileName,   
                                          dayDirectory + "\\" + standardFileName + selectedImage.CaptureTime.ToString("yy_MM_dd_") + 
                                            imageIndex.ToString("0000") + ".NEF");
                            imageIndex++;
                        }
                    }
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ImagesByDaySummary daySummary = (ImagesByDaySummary)listView1.SelectedItems[0].Tag;
                checkBoxDayCfgActive.Checked = daySummary.CfgActive;
                numericDayCfgIntSec.Value = daySummary.CfgIntervalSec;
                numericDayCfgStartHour.Value = daySummary.CfgStartTime / 100;
                numericDayCfgStartMinute.Value = daySummary.CfgStartTime % 100;
                numericDayCfgEndHour.Value = daySummary.CfgEndTime / 100;
                numericDayCfgEndMinute.Value = daySummary.CfgEndTime % 100;

                mSelectedDay = daySummary;
                checkBoxDayCfgActive.Enabled = numericDayCfgIntSec.Enabled = numericDayCfgStartHour.Enabled =
                    numericDayCfgStartMinute.Enabled = numericDayCfgEndHour.Enabled = numericDayCfgEndMinute.Enabled = true;
                groupBoxDaySettings.Text = "Day Settings - " + daySummary.FirstTime.ToShortDateString();
            }
            else if(listView1.SelectedItems.Count > 1)
            {
                checkBoxDayCfgActive.Enabled = numericDayCfgIntSec.Enabled = numericDayCfgStartHour.Enabled =
                    numericDayCfgStartMinute.Enabled = numericDayCfgEndHour.Enabled = numericDayCfgEndMinute.Enabled = true;
                groupBoxDaySettings.Text = "Day settings - multiple selected";
            }
            else // No items selected
            {
                mSelectedDay = null;
                checkBoxDayCfgActive.Enabled = numericDayCfgIntSec.Enabled = numericDayCfgStartHour.Enabled =
                    numericDayCfgStartMinute.Enabled = numericDayCfgEndHour.Enabled = numericDayCfgEndMinute.Enabled = false;
                groupBoxDaySettings.Text = "Day Settings";
            }
        }

        private void checkBoxDayCfgActive_CheckedChanged(object sender, EventArgs e)
        {
            if (mSelectedDay != null)
            {
                foreach (ListViewItem listItem in listView1.SelectedItems)
                {
                    ImagesByDaySummary imagesByDaySummary = (ImagesByDaySummary)listItem.Tag;
                    imagesByDaySummary.CfgActive = checkBoxDayCfgActive.Checked;
                }
            }
        }

        private void numericDayCfgIntSec_ValueChanged(object sender, EventArgs e)
        {
            if (mSelectedDay != null)
            {
                foreach (ListViewItem listItem in listView1.SelectedItems)
                {
                    ImagesByDaySummary imagesByDaySummary = (ImagesByDaySummary)listItem.Tag;
                    imagesByDaySummary.CfgIntervalSec = (int)numericDayCfgIntSec.Value;
                }
            }
        }

        private void numericDayCfgStartHour_ValueChanged(object sender, EventArgs e)
        {
            if (mSelectedDay != null)
            {
                foreach (ListViewItem listItem in listView1.SelectedItems)
                {
                    ImagesByDaySummary imagesByDaySummary = (ImagesByDaySummary)listItem.Tag;
                    imagesByDaySummary.CfgStartTime = (int)(numericDayCfgStartHour.Value * 100 + numericDayCfgStartMinute.Value);
                }
            }
        }

        private void numericDayCfgEndHour_ValueChanged(object sender, EventArgs e)
        {
            if (mSelectedDay != null)
            {
                foreach (ListViewItem listItem in listView1.SelectedItems)
                {
                    ImagesByDaySummary imagesByDaySummary = (ImagesByDaySummary)listItem.Tag;
                    imagesByDaySummary.CfgEndTime = (int)(numericDayCfgEndHour.Value * 100 + numericDayCfgEndMinute.Value);
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            updateImageRecordStatus(false);
        }

        private void buttonSetAllActive_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<DateTime, ImagesByDaySummary> entry in mImageRecordsByDay)
            {
                entry.Value.CfgActive = true;
            }
            updateImageRecordStatus(false);
        }

        private void checkBoxShowInactive_CheckedChanged(object sender, EventArgs e)
        {
            updateImageRecordStatus(false);
        }

        private void buttonSaveSettingsToFile_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text != null && comboBox1.Text != "")
            {
                string daySettingFile = Path.GetDirectoryName(openImageFile) + "\\" + comboBox1.Text +
                                        ".llods";

                logMessage("Storing day settings to: " + daySettingFile);
                using (FileStream fs = File.OpenWrite(daySettingFile))
                {
                    fs.Write(BitConverter.GetBytes(FILE_DAYCFG_MAGIC_NUMBER), 0, 4);
                    foreach (KeyValuePair<DateTime, ImagesByDaySummary> entry in mImageRecordsByDay)
                    {
                        fs.Write(entry.Value.getDaySettingBytes(), 0, 20);        
                    }
                }
            }
        }

        private void buttonLoadSettings_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dayCfgSettingFile = Path.GetDirectoryName(openImageFile) + "\\" + comboBox1.Text + ".llods";
            using (FileStream fs = File.OpenRead(dayCfgSettingFile))
            {
                byte[] magicNumber = new byte[4];
                byte[] dayCfgBuffer = new byte[20];
                fs.Read(magicNumber, 0, 4);
                if (BitConverter.ToInt32(magicNumber, 0) == FILE_DAYCFG_MAGIC_NUMBER)
                {
                    while (fs.Read(dayCfgBuffer, 0, 20) == 20)
                    {
                        DateTime day = DateTime.FromBinary(BitConverter.ToInt64(dayCfgBuffer, 0));
                        if (mImageRecordsByDay.ContainsKey(day))
                        {
                            mImageRecordsByDay[day].loadDaySettingsFromBytes(dayCfgBuffer);
                        }
                    }
                    updateImageRecordStatus(false);
                    logMessage("Day settings loaded from " + dayCfgSettingFile);
                }
            }
        }
    }

    public class ImagesByDaySummary
    {
        public DateTime Day;
        public DateTime FirstTime; 
        public DateTime LastTime;

        // Day settings
        public int  CfgStartTime;
        public int  CfgEndTime;
        public bool CfgActive;
        public int  CfgIntervalSec;

        public int NumberOfPictures;
        public List<ImageRecord> SelectedImages;
        public int []numImagesByHour = new int[24];

        // Static selection variables
        public static int SelectedStartHour = 8;
        public static int SelectedEndHour = 17;
        public static int SelectedMinIntervalSeconds = 60;
        private static ImageRecord SelectedPreviousImageRecord = null;

        public ImagesByDaySummary(DateTime imageDT)
        {
            NumberOfPictures = 1;
            FirstTime = imageDT;
            LastTime = imageDT;
            CfgStartTime = 8 * 100;
            CfgEndTime = 18 * 100;
            CfgActive = true;
            CfgIntervalSec = 60;

            SelectedImages = new List<ImageRecord>();
            for (int i = 0; i < 24; i++) numImagesByHour[i] = 0;
            registerDateTime(imageDT);
        }

        public void registerDateTime(DateTime imageDT)
        {
            if (FirstTime > imageDT) FirstTime = imageDT;
            if (LastTime < imageDT) LastTime = imageDT;
            numImagesByHour[imageDT.Hour]++;
        }

        public void addImage(ImageRecord ir)
        {
            int captureTimeStamp = ir.CaptureTime.Hour * 100 + ir.CaptureTime.Minute;
            if(captureTimeStamp >= CfgStartTime && captureTimeStamp <= CfgEndTime)
            {
                if (SelectedPreviousImageRecord != null)
                {
                    if (Math.Abs((SelectedPreviousImageRecord.CaptureTime - ir.CaptureTime).TotalSeconds) > (double)CfgIntervalSec)
                    {
                        SelectedImages.Add(ir);
                        SelectedPreviousImageRecord = ir;
                    }
                }
                else
                {
                    SelectedImages.Add(ir);
                    SelectedPreviousImageRecord = ir;
                }
            }
        }

        public void clearImages()
        {
            SelectedImages.Clear();
        }

        public string getNumImagesByHourString()
        {
            string returnString = "";
            for(int i = 0; i < 23; i++)
            {
                returnString += numImagesByHour[i].ToString();
                returnString += "-";
            }
            returnString += numImagesByHour[23].ToString();
            return returnString;
        }

        public byte [] getDaySettingBytes()
        {
            byte[] retBytes = new byte[20];
            BitConverter.GetBytes(Day.Ticks).CopyTo(retBytes, 0);
            retBytes[8] = (byte)(CfgActive ? 1 : 0);
            BitConverter.GetBytes((int)CfgIntervalSec).CopyTo(retBytes, 12);
            BitConverter.GetBytes((short)CfgStartTime).CopyTo(retBytes, 16);
            BitConverter.GetBytes((short)CfgEndTime).CopyTo(retBytes, 18);
            return retBytes;
        }

        public void loadDaySettingsFromBytes(byte[] rawBuffer)
        {
            CfgActive = (rawBuffer[8] > 0);
            CfgIntervalSec = BitConverter.ToInt32(rawBuffer, 12);
            CfgStartTime = BitConverter.ToInt16(rawBuffer, 16);
            CfgEndTime = BitConverter.ToInt16(rawBuffer, 18);
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
            newImageRecord.CaptureTime = newImageRecord.CaptureTime.AddHours(11);
            newImageRecord.FileName = System.Text.Encoding.Default.GetString(name);
            return newImageRecord;
        }
    }
}
