using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace PhotoViewer
{
    public partial class Main : Form
    {
        private string _filePath;
        public Main()
        {
            InitializeComponent();
            _filePath = Properties.Settings.Default.StartPicturePath;
            SetBtnVisibility();
            DisplayStartPicture();
        }

        private void DisplayStartPicture()
        {
            if (!String.IsNullOrWhiteSpace(_filePath))
            {
                DisplayPicture();
                SetBtnVisibility();
            };
        }

        private void DisplayPicture()
        {
            var startPicture = Image.FromFile(_filePath);
            pbxDisplay.Image = startPicture;
            pbxDisplay.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void SetBtnVisibility()
        {
            if (String.IsNullOrWhiteSpace(_filePath))
            {
                btnRemovePicture.Visible = false;
                btnPrint.Visible = false;
            }

            else
            {
                btnRemovePicture.Visible = true;
                btnPrint.Visible = true;
            }
        }

        private void btnLoadPicture_Click(object sender, EventArgs e)
        {
            pbxDisplay.SizeMode = PictureBoxSizeMode.Zoom;
            openFileDialog1.ShowDialog();
            _filePath = openFileDialog1.FileName;
            DisplayPicture();
            SetStartPicturePath();
            SetBtnVisibility();
        }

        private void btnRemovePicture_Click(object sender, EventArgs e)
        {
            pbxDisplay.Image = null;
            _filePath = null;
            SetStartPicturePath();
            SetBtnVisibility();
        }

        private void SetStartPicturePath()
        {
            Properties.Settings.Default.StartPicturePath = _filePath;
            Properties.Settings.Default.Save();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += DocPrintPage;
            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void DocPrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(pbxDisplay.Width, pbxDisplay.Height);
            pbxDisplay.DrawToBitmap(bm, new Rectangle(0, 0, pbxDisplay.Width, pbxDisplay.Height));
            e.Graphics.DrawImage(bm, 0, 0);
            bm.Dispose();
        }
    }
}
