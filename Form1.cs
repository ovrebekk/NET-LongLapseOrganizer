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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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
                        if (mImageRecordsByDay.ContainsKey(ir.CaptureTimeAdjusted.Date))
                        {
                            mImageRecordsByDay[ir.CaptureTimeAdjusted.Date].NumberOfPictures++;
                            mImageRecordsByDay[ir.CaptureTimeAdjusted.Date].registerDateTime(ir.CaptureTimeAdjusted);
                            mImageRecordsByDay[ir.CaptureTimeAdjusted.Date].addImage(ir);
                        }
                        else
                        {
                            ImagesByDaySummary summary = new ImagesByDaySummary(ir.CaptureTimeAdjusted);
                            summary.addImage(ir);
                            summary.Day = ir.CaptureTimeAdjusted.Date;
                            mImageRecordsByDay.Add(ir.CaptureTimeAdjusted.Date, summary);
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
                        mImageRecordsByDay[ir.CaptureTimeAdjusted.Date].addImage(ir);
                    }
                }
                int selectedIndex = -1;
                if (listViewMain.SelectedItems.Count == 1) selectedIndex = listViewMain.SelectedIndices[0];
                listViewMain.Items.Clear();
                foreach (KeyValuePair<DateTime, ImagesByDaySummary> entry in mImageRecordsByDay)
                {
                    if (checkBoxShowInactive.Checked || entry.Value.CfgActive)
                    {
                        ListViewItem listViewItem = new ListViewItem(entry.Value.getStringsForList());
                        listViewItem.Tag = entry.Value;
                        listViewMain.Items.Add(listViewItem);
                    }
                    if (entry.Value.CfgActive) selectedImagesTotal += entry.Value.SelectedImages.Count;
                }
                if (selectedIndex != -1)
                {
                    listViewMain.Items[selectedIndex].Selected = true;
                    listViewMain.EnsureVisible(selectedIndex);
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
            catch (Exception exception)
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
            foreach (String file in localFiles)
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
            foreach (ImageRecord ir in mImageRecordList)
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
            if (saveFileDialog1.FileName != null)
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
            if (openFileDialog1.FileName != null && openFileDialog1.FileName != "")
            {
                logMessage("Loading " + openFileDialog1.FileName);
                openImageFile = openFileDialog1.FileName;
                byte[] fileData = File.ReadAllBytes(openFileDialog1.FileName);

                if (BitConverter.ToInt32(fileData, 0) == FILE_MAGIC_NUMBER)
                {
                    byte[] uncompressedBytes = new byte[fileData.Length - 4];
                    Array.Copy(fileData, 4, uncompressedBytes, 0, fileData.Length - 4);
                    using (var inStream = new MemoryStream(uncompressedBytes))
                    {
                        using (var archive = new ZipArchive(inStream, ZipArchiveMode.Read, true))
                        {
                            var fileInArchive = archive.GetEntry("filelist_zipped");

                            Stream unzippedEntryStream = fileInArchive.Open();
                            mImageRecordList.Clear();
                            while (true)
                            {
                                ImageRecord newRecord = ImageRecord.ImageRecordFromStream(unzippedEntryStream);
                                if (newRecord == null) break;
                                mImageRecordList.Add(newRecord);
                            }
                        }
                    }

                    logMessage(mImageRecordList.Count.ToString() + " records loaded.");

                    updateImageRecordStatus(true);

                    buttonSaveFilelist.Enabled = true;

                    // Populate combobox of day setting files based on files in the same directory
                    comboBoxDaySettingFiles.Items.Clear();
                    string[] daySettingFiles = System.IO.Directory.GetFiles(Path.GetDirectoryName(openFileDialog1.FileName),
                                                                            "*.llods", SearchOption.AllDirectories);
                    foreach (String file in daySettingFiles)
                    {
                        comboBoxDaySettingFiles.Items.Add(Path.GetFileNameWithoutExtension(file));
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
            else if (!System.IO.Directory.Exists(textBoxOutputFolder.Text))
            {
                logMessage("Error: Output directory doesn't exist!");
            }
            else
            {
                foreach (KeyValuePair<DateTime, ImagesByDaySummary> dayEntry in mImageRecordsByDay)
                {
                    if (dayEntry.Value.CfgActive)
                    {
                        string dayDirectory = textBoxOutputFolder.Text + "\\" + dayEntry.Key.ToString("yy_MM_dd");
                        System.IO.Directory.CreateDirectory(dayDirectory);
                        int imageIndex = 1;
                        foreach (ImageRecord selectedImage in dayEntry.Value.SelectedImages)
                        {
                            createSymLink(selectedImage.FileName,
                                          dayDirectory + "\\" + standardFileName + selectedImage.CaptureTimeAdjusted.ToString("yy_MM_dd_") +
                                            imageIndex.ToString("0000") + ".NEF");
                            imageIndex++;
                        }
                    }
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewMain.SelectedItems.Count == 1)
            {
                ImagesByDaySummary daySummary = (ImagesByDaySummary)listViewMain.SelectedItems[0].Tag;
                checkBoxDayCfgActive.Checked = daySummary.CfgActive;
                numericDayCfgIntSec.Value = daySummary.CfgIntervalSec;
                numericDayCfgStartHour.Value = daySummary.CfgStartTime / 100;
                numericDayCfgStartMinute.Value = daySummary.CfgStartTime % 100;
                numericDayCfgEndHour.Value = daySummary.CfgEndTime / 100;
                numericDayCfgEndMinute.Value = daySummary.CfgEndTime % 100;

                mSelectedDay = daySummary;
                checkBoxDayCfgActive.Enabled = numericDayCfgIntSec.Enabled = numericDayCfgStartHour.Enabled =
                    numericDayCfgStartMinute.Enabled = numericDayCfgEndHour.Enabled = numericDayCfgEndMinute.Enabled = true;
                labelDaySelected.Text = daySummary.FirstTime.ToShortDateString();

                // Load list of all images for this day
                listViewImages.Items.Clear();
                ImagesByDaySummary summary = (ImagesByDaySummary)listViewMain.SelectedItems[0].Tag;
                DateTime prevImageDateTime = DateTime.MinValue;
                foreach (ImageRecord ir in summary.AllImages)
                {
                    ListViewItem newItem = new ListViewItem(ir.getListStrings(prevImageDateTime));
                    newItem.Tag = ir;
                    listViewImages.Items.Add(newItem);
                    prevImageDateTime = ir.CaptureTimeAdjusted;
                }

                // Simulate button click to update picture summary form, if it is enabled
                button2_Click_1(null, null);
            }
            else if (listViewMain.SelectedItems.Count > 1)
            {
                checkBoxDayCfgActive.Enabled = numericDayCfgIntSec.Enabled = numericDayCfgStartHour.Enabled =
                    numericDayCfgStartMinute.Enabled = numericDayCfgEndHour.Enabled = numericDayCfgEndMinute.Enabled = true;
                labelDaySelected.Text = listViewMain.SelectedItems.Count.ToString() + " days selected";
            }
            else // No items selected
            {
                mSelectedDay = null;
                checkBoxDayCfgActive.Enabled = numericDayCfgIntSec.Enabled = numericDayCfgStartHour.Enabled =
                    numericDayCfgStartMinute.Enabled = numericDayCfgEndHour.Enabled = numericDayCfgEndMinute.Enabled = false;
                labelDaySelected.Text = "-";
            }
        }

        private void checkBoxDayCfgActive_CheckedChanged(object sender, EventArgs e)
        {
            if (mSelectedDay != null)
            {
                foreach (ListViewItem listItem in listViewMain.SelectedItems)
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
                foreach (ListViewItem listItem in listViewMain.SelectedItems)
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
                foreach (ListViewItem listItem in listViewMain.SelectedItems)
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
                foreach (ListViewItem listItem in listViewMain.SelectedItems)
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
            if (comboBoxDaySettingFiles.Text != null && comboBoxDaySettingFiles.Text != "")
            {
                string daySettingFile = Path.GetDirectoryName(openImageFile) + "\\" + comboBoxDaySettingFiles.Text +
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
            string dayCfgSettingFile = Path.GetDirectoryName(openImageFile) + "\\" + comboBoxDaySettingFiles.Text + ".llods";
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                if (listViewMain.SelectedItems.Count == 1)
                {
                    int index = listViewMain.SelectedIndices[0];
                    if (index > 0)
                    {
                        listViewMain.Items[index].Selected = false;
                        listViewMain.Items[index - 1].Selected = true;
                        listViewMain.EnsureVisible(index - 1);
                    }
                }
            }
            else if (e.KeyCode == Keys.D)
            {
                if (listViewMain.SelectedItems.Count == 1)
                {
                    int index = listViewMain.SelectedIndices[0];
                    if (index < (listViewMain.Items.Count - 1))
                    {
                        listViewMain.Items[index].Selected = false;
                        listViewMain.Items[index + 1].Selected = true;
                        listViewMain.EnsureVisible(index + 1);
                    }
                }
            }
        }

        private void buttonLoadImages_Click(object sender, EventArgs e)
        {

        }

        private void listViewImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewImages.SelectedItems.Count == 1)
            {
                ImageRecord imageRecord = (ImageRecord)listViewImages.SelectedItems[0].Tag;
                pictureBoxPreview.Image = getThumbnailFromImageRecord(imageRecord);
            }
        }

        private Image getThumbnailFromImageRecord(ImageRecord ir)
        {
            Image returnImage;
            if ((returnImage = ImageProcessing.getJpgThumbnailFromFile(ir.FileName)) != null)
            {
                ir.ThumbnailStored = true;
                return returnImage;
            }
            else if ((returnImage = ImageProcessing.getJpgThumbnailFromNEF(ir.FileName)) != null)
            {
                Console.WriteLine("Thumb not found, generated: " + ir.FileName);
                ImageProcessing.saveJpgThumbnailToFile(returnImage, ir.FileName);
                ir.ThumbnailStored = true;
                return returnImage;
            }
            else
            {
                logMessage("Error: Can't generate JPG thumbnail from NEF");
                return null;
            }
        }

        private void buttonDisplayImage_Click(object sender, EventArgs e)
        {

        }

        private void buttonLoadAllThumbs_Click(object sender, EventArgs e)
        {
            if(listViewMain.SelectedItems.Count == 1)
            {
                ImagesByDaySummary summary = (ImagesByDaySummary)listViewMain.SelectedItems[0].Tag;
                List<string> filenameList = new List<string>();
                List<string> filenameList2 = new List<string>();
                List<string> filenameList3 = new List<string>();
                List<string> filenameList4 = new List<string>();
                int i = 0;
                foreach (ImageRecord ir in summary.AllImages)
                {
                    switch(i%4)
                    {
                        case 0: filenameList.Add(ir.FileName); break;
                        case 1: filenameList2.Add(ir.FileName); break;
                        case 2: filenameList3.Add(ir.FileName); break;
                        case 3: filenameList4.Add(ir.FileName); break;
                        default:break;
                    }
                    i++;
                }
                ImageProcessing.startProcessingThread(filenameList.ToArray());
                ImageProcessing.startProcessingThread(filenameList2.ToArray());
                ImageProcessing.startProcessingThread(filenameList3.ToArray());
                ImageProcessing.startProcessingThread(filenameList4.ToArray());
            }
        }

        private void buttonPicTimeFirst_Click(object sender, EventArgs e)
        {
            if(listViewImages.SelectedItems.Count == 1)
            {
                ImageRecord ir = (ImageRecord)listViewImages.SelectedItems[0].Tag;
                numericDayCfgStartHour.Value = ir.CaptureTimeAdjusted.Hour;
                numericDayCfgStartMinute.Value = ir.CaptureTimeAdjusted.Minute;
            }
        }

        private void buttonPicTimeLast_Click(object sender, EventArgs e)
        {
            if (listViewImages.SelectedItems.Count == 1)
            {
                ImageRecord ir = (ImageRecord)listViewImages.SelectedItems[0].Tag;
                numericDayCfgEndHour.Value = ir.CaptureTimeAdjusted.Hour;
                numericDayCfgEndMinute.Value = ir.CaptureTimeAdjusted.Minute;
            }
        }

        DayBoundaryView dayBoundaryView = null;

        private void button2_Click_1(object sender, EventArgs e)
        {
            if(sender != null && dayBoundaryView == null)
            {
                dayBoundaryView = new DayBoundaryView();
                dayBoundaryView.Show();
            }
            if (dayBoundaryView != null && listViewMain.SelectedItems.Count == 1)
            {
                Image[] listImages = new Image[6];
                ImagesByDaySummary daySummary;
                if (listViewMain.SelectedIndices[0] > 0)
                {
                    daySummary = (ImagesByDaySummary)listViewMain.Items[listViewMain.SelectedIndices[0] - 1].Tag;
                    listImages[0] = getThumbnailFromImageRecord(daySummary.AllImages.First());
                    listImages[1] = getThumbnailFromImageRecord(daySummary.AllImages.Last());
                }
                daySummary = (ImagesByDaySummary)listViewMain.Items[listViewMain.SelectedIndices[0]].Tag;
                listImages[2] = getThumbnailFromImageRecord(daySummary.AllImages.First());
                listImages[3] = getThumbnailFromImageRecord(daySummary.AllImages.Last());
                if (listViewMain.SelectedIndices[0] < (listViewMain.Items.Count - 1))
                {
                    daySummary = (ImagesByDaySummary)listViewMain.Items[listViewMain.SelectedIndices[0] + 1].Tag;
                    listImages[4] = getThumbnailFromImageRecord(daySummary.AllImages.First());
                    listImages[5] = getThumbnailFromImageRecord(daySummary.AllImages.Last());
                }
                dayBoundaryView.setImages(listImages);
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
        public List<ImageRecord> AllImages;
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
            AllImages = new List<ImageRecord>();
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
            int captureTimeStamp = ir.CaptureTimeAdjusted.Hour * 100 + ir.CaptureTimeAdjusted.Minute;
            if(captureTimeStamp >= CfgStartTime && captureTimeStamp <= CfgEndTime)
            {
                if (SelectedPreviousImageRecord != null)
                {
                    if (Math.Abs((SelectedPreviousImageRecord.CaptureTimeAdjusted - ir.CaptureTimeAdjusted).TotalSeconds) >= (double)CfgIntervalSec)
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
            AllImages.Add(ir);
        }

        public void clearImages()
        {
            SelectedImages.Clear();
            AllImages.Clear();
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

        public string [] getStringsForList()
        {
            string[] newListStrings = new string[10];
            newListStrings[0] = Day.ToShortDateString();
            newListStrings[1] = NumberOfPictures.ToString();
            newListStrings[2] = FirstTime.ToShortTimeString();
            newListStrings[3] = LastTime.ToShortTimeString();
            newListStrings[4] = SelectedImages.Count.ToString();
            newListStrings[5] = CfgActive ? "Yes" : "-";
            newListStrings[6] = CfgIntervalSec.ToString();
            newListStrings[7] = CfgStartTime.ToString();
            newListStrings[8] = CfgEndTime.ToString();
            newListStrings[9] = getNumImagesByHourString();
            return newListStrings;
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
        private Int16 mDateTimeAdjustMinutes;
        private bool mActive;
        private bool mThumbnailStored;

        public ImageRecord()
        {
            mActive = true;
            mThumbnailStored = false;
            mDateTimeAdjustMinutes = 0;
        }

        public DateTime CaptureTime
        {
            //get { return mDateTime; }
            set { mDateTime = value; }
        }

        public DateTime CaptureTimeAdjusted
        {
            get { return mDateTime.AddMinutes(mDateTimeAdjustMinutes); }
        }

        public Int16 DateTimeAdjustMinutes
        {
            set { mDateTimeAdjustMinutes = value; }
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

        public bool ThumbnailStored
        {
            get { return mThumbnailStored; }
            set { mThumbnailStored = value; }
        }

        // File related
        public int getByteLength()
        {
            return Encoding.ASCII.GetBytes(mFileName).Length + 2 // FileName
                   + 8                  // DateTime
                   + 2                  // DateTimeAdjustMinutes
                   + 1                  // bool Active
                   + 1;                 // bool ThumbnailStored
        }

        public byte [] getBytes()
        {
            int arrayLength = getByteLength();
            byte[] returnArray = new byte[arrayLength];
            returnArray[0] = mActive ? (byte)1 : (byte)0;
            returnArray[1] = mThumbnailStored ? (byte)1 : (byte)0;
            BitConverter.GetBytes(mDateTime.Ticks).CopyTo(returnArray, 2);
            BitConverter.GetBytes(mDateTimeAdjustMinutes).CopyTo(returnArray, 10);
            BitConverter.GetBytes((UInt16)(mFileName.Length)).CopyTo(returnArray, 12);
            Encoding.ASCII.GetBytes(mFileName).CopyTo(returnArray, 14);
            return returnArray;
        }

        public string [] getListStrings(DateTime lastPicTimestamp)
        {
            string[] retStrings = new string[4];
            retStrings[0] = CaptureTimeAdjusted.ToString("HH:mm:ss");
            if (lastPicTimestamp != DateTime.MinValue) retStrings[1] = ((int)((mDateTime.Ticks - lastPicTimestamp.Ticks) / 10000000)).ToString();
            else retStrings[1] = "-";
            retStrings[2] = mDateTimeAdjustMinutes.ToString();
            retStrings[3] = mFileName;
            return retStrings;
        }

        // Sorting
        public int CompareTo(ImageRecord other)
        {
            return CaptureTimeAdjusted.CompareTo(other.CaptureTimeAdjusted);
        }

        static public ImageRecord ImageRecordFromStream(Stream inputStream)
        {
            ImageRecord newImageRecord = new ImageRecord();
            byte[] active = new byte[1];
            byte[] thumbnailSaved = new byte[1];
            byte[] dateTime = new byte[8];
            byte[] dateTimeAdjustMinutes = new byte[2];
            byte[] nameLength = new byte[2];
            if(inputStream.Read(active, 0, 1) == 0) return null;
            if(inputStream.Read(thumbnailSaved, 0, 1) == 0) return null;
            if(inputStream.Read(dateTime, 0, 8) == 0) return null;
            if(inputStream.Read(dateTimeAdjustMinutes, 0, 2) == 0) return null;
            if(inputStream.Read(nameLength, 0, 2) == 0) return null;
            byte[] name = new byte[BitConverter.ToUInt16(nameLength, 0)];
            if(inputStream.Read(name, 0, BitConverter.ToUInt16(nameLength, 0)) == 0) return null;
            newImageRecord.Active = (active[0] != 0);
            newImageRecord.ThumbnailStored = (thumbnailSaved[0] != 0);
            newImageRecord.CaptureTime = DateTime.FromBinary(BitConverter.ToInt64(dateTime, 0));
            Int16 adjustMinutes = BitConverter.ToInt16(dateTimeAdjustMinutes, 0);
            newImageRecord.DateTimeAdjustMinutes = adjustMinutes;
            newImageRecord.FileName = System.Text.Encoding.Default.GetString(name);
            return newImageRecord;
        }
    }
}
