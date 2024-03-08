using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        int index = 1;
        bool fullScreen = false;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Visible = false;
        }

        // Start Button Functionality.
        private void startbutton_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            if (index == 1)
            {
                LoadImageWithTransition(index);
                index++;
            }
            else
            {
                MessageBox.Show("Click Next Button For More Slides");
            }
        }

        // Loading Image with Fade-In Transition.
        private async void LoadImageWithTransition(int index)
        {
            String folderPath = @"D:\picture\";
            String imagePath = "Capture" + index + ".PNG";
            String fullPath = Path.Combine(folderPath, imagePath);

            if (File.Exists(fullPath))
            {
                Image newImage = Image.FromFile(fullPath);

                // Fade out previous image if exists
                if (pictureBox1.Image != null)
                {
                    for (double opacity = 1.0; opacity >= 0; opacity -= 0.05)
                    {
                        await Task.Delay(20);
                        pictureBox1.Image = AdjustOpacity((Bitmap)pictureBox1.Image, opacity);
                    }
                }

                // Display new image
                pictureBox1.Image = newImage;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                // Fade in new image
                for (double opacity = 0.0; opacity <= 1.0; opacity += 0.05)
                {
                    await Task.Delay(20);
                    pictureBox1.Image = AdjustOpacity((Bitmap)newImage, opacity);
                }
            }
            else
            {
                MessageBox.Show("Image Not Found !");
            }
        }

        // Adjust Opacity of Image.
        private Bitmap AdjustOpacity(Bitmap image, double opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = (float)opacity;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();
            return bmp;
        }

        // Next Button Functionality.
        private void button3_Click(object sender, EventArgs e)
        {
            LoadImageWithTransition(index);
            index++;
        }

        // Previous Button Functionality.
        private void previousButton_Click(object sender, EventArgs e)
        {
            index--;
            LoadImageWithTransition(index);
        }

        // Zoom In Functionality.
        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Width = (int)(pictureBox1.Width * 1.1);
            pictureBox1.Height = (int)(pictureBox1.Height * 1.1);
        }

        // Zoom Out Functionality.
        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.Width = (int)(pictureBox1.Width / 1.1);
            pictureBox1.Height = (int)(pictureBox1.Height / 1.1);
        }

        // Full Screen Functionality.
        private void fullScreenButton_Click(object sender, EventArgs e)
        {
            if (fullScreen == false)
            {
                this.WindowState = FormWindowState.Maximized;
                fullScreen = true;
            }
            else if (fullScreen == true)
            {
                this.WindowState = FormWindowState.Normal;
                fullScreen = false;
            }
        }
    }
}