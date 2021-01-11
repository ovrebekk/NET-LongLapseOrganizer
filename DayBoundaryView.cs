using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LongLapseOrganizer
{
    public partial class DayBoundaryView : Form
    {
        public DayBoundaryView()
        {
            InitializeComponent();
        }

        private void DayBoundaryView_Load(object sender, EventArgs e)
        {

        }

        public void setImages(Image []imageList)
        {
            pictureBox1A.Image = imageList[0];
            pictureBox1B.Image = imageList[1];
            pictureBox2A.Image = imageList[2];
            pictureBox2B.Image = imageList[3];
            pictureBox3A.Image = imageList[4];
            pictureBox3B.Image = imageList[5];
        }
    }
}
