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

namespace LongLapseOrganizer
{
    public partial class Form1 : Form
    {
        private string[] localFiles;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            localFiles = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.NEF");
            Console.WriteLine("Found " + localFiles.Length + " .NEF files");

        }
    }
}
