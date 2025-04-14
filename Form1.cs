using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Lab2_v1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Image<Bgr, byte> sourceImage;
        private Image<Bgr, byte> sourceImage2;
        private Image<Bgr, byte> bgrImage;
        private Image<Bgr, byte> bufer1;
        private Image<Bgr, byte> bufer2;
        private Image<Bgr, byte> bufer3;
        private Image<Gray, byte> bufer1g;
        private Image<Gray, byte> bufer2g;


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();   
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                sourceImage = new Image<Bgr, byte>(fileName);
            }
            imageBox1.Image = sourceImage.Resize(640, 480, Inter.Linear);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var channel = sourceImage.Split()[2];
            imageBox2.Image = channel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var channel = sourceImage.Split()[1];
            imageBox2.Image = channel;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var channel = sourceImage.Split()[0];
            imageBox2.Image = channel;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> grayImage = sourceImage.Convert<Gray, byte>();

            var tempImage = grayImage.PyrDown();
            var destImage = tempImage.PyrUp();

            VectorOfMat vm = new VectorOfMat();
            vm.Push(sourceImage.Split()[0]); vm.Push(sourceImage.Split()[1]); vm.Push(sourceImage.Split()[2]);
            CvInvoke.Merge(vm, destImage);

            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                    grayImage.Data[y, x, 0] = Convert.ToByte(0.299 * sourceImage.Data[y, x, 2] + 0.587 * sourceImage.Data[y, x, 1] + 0.114 * sourceImage.Data[y, x, 0]);

            imageBox3.Image = grayImage;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Image<Bgr,byte> imgCopy = sourceImage.CopyBlank();
            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    byte red = sourceImage.Data[x,y,2];
                    byte green = sourceImage.Data[x,y,1];
                    byte blue = sourceImage.Data[x,y,0];

                    byte redRes = Convert.ToByte(Math.Min((red * 0.393 + green * 0.769 + blue * 0.189), 255));
                    byte greenRes = Convert.ToByte(Math.Min((red * 0.349 + green * 0.686 + blue * 0.168), 255));
                    byte blueRes = Convert.ToByte(Math.Min((red * 0.272 + green * 0.534 + blue * 0.131), 255));
                    imgCopy.Data[x, y, 0] = blueRes;
                    imgCopy.Data[x, y, 1] = greenRes;
                    imgCopy.Data[x, y, 2] = redRes;
                }
            imageBox2.Image = imgCopy;
        }

        private float contrast = 1.0f;
        private float brightness = 0.0f;

        private void button7_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> imgCopy = sourceImage.CopyBlank();
            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    byte red = sourceImage.Data[x, y, 2];
                    byte green = sourceImage.Data[x, y, 1];
                    byte blue = sourceImage.Data[x, y, 0];

                    //byte redRes = Convert.ToByte(red * contrast + brightness);
                    //byte greenRes = Convert.ToByte(green * contrast + brightness);
                    //byte blueRes = Convert.ToByte(blue * contrast + brightness);

                    byte redRes = Convert.ToByte(Math.Min((red * contrast + brightness), 255));
                    byte greenRes = Convert.ToByte(Math.Min((green * contrast + brightness), 255));
                    byte blueRes = Convert.ToByte(Math.Min((blue * contrast + brightness), 255));

                    imgCopy.Data[x, y, 0] = blueRes;
                    imgCopy.Data[x, y, 1] = greenRes;
                    imgCopy.Data[x, y, 2] = redRes;
                }
            imageBox2.Image = imgCopy;
            bufer1 = imgCopy;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            contrast = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            brightness = trackBar2.Value * 10;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                sourceImage2 = new Image<Bgr, byte>(fileName);
            }
            imageBox3.Image = sourceImage2.Resize(640, 480, Inter.Linear);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> imgCopy;
            if (bufer2 != null)
            {
                imgCopy = bufer2.Resize(736, 736, Inter.Linear);
            }
            else
            {
                imgCopy = sourceImage.CopyBlank().Resize(736, 736, Inter.Linear);
            }
            //Image<Bgr, byte> imgCopy = sourceImage.CopyBlank();
            Image<Bgr, byte> imgCopy2 = sourceImage2.CopyBlank();

            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    byte red = imgCopy.Data[x, y, 2];
                    byte green = imgCopy.Data[x, y, 1];
                    byte blue = imgCopy.Data[x, y, 0];

                    //byte red = sourceImage.Data[x, y, 2];
                    //byte green = sourceImage.Data[x, y, 1];
                    //byte blue = sourceImage.Data[x, y, 0];

                    byte red2 = sourceImage2.Data[x, y, 2];
                    byte green2 = sourceImage2.Data[x, y, 1];
                    byte blue2 = sourceImage2.Data[x, y, 0];

                    imgCopy2.Data[x, y, 0] = AddColors(blue, blue2);
                    imgCopy2.Data[x, y, 1] = AddColors(green, green2);
                    imgCopy2.Data[x, y, 2] = AddColors(red, red2);
                }

            imageBox2.Image = imgCopy2;
            bufer3 = imgCopy2;
        }

        private byte AddColors(byte color1, byte color2)
        {
            if (color1 + color2 > 255) return 255;
            else if (color1 + color2 < 0) return 0;
            //else return Convert.ToByte(color1*(1 + trackBar5.Value/10) + color2*trackBar5.Value/10);
            else return Convert.ToByte(color1 + color2);
        }
        private byte MinusColors(byte color1, byte color2)
        {
            if (color1 - color2 < 0) return 0;
            else if (color1 - color2 > 255) return 255;
            else return Convert.ToByte(color1 - color2);
        }
        private byte MultiplyColors(byte color1, byte color2)
        {
            if (color1 == color2) return color1;
            else return 0;
        }
        private byte MultiplyColors1(byte color1, byte color2)
        {
            byte result = (byte) (color1 * color2);
           return result;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> imgCopy = sourceImage.CopyBlank();
            Image<Bgr, byte> imgCopy2 = sourceImage2.CopyBlank();
            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    byte red = sourceImage.Data[x, y, 2];
                    byte green = sourceImage.Data[x, y, 1];
                    byte blue = sourceImage.Data[x, y, 0];

                    byte red2 = sourceImage2.Data[x, y, 2];
                    byte green2 = sourceImage2.Data[x, y, 1];
                    byte blue2 = sourceImage2.Data[x, y, 0];

                    imgCopy2.Data[x, y, 0] = MinusColors(blue, blue2);
                    imgCopy2.Data[x, y, 1] = MinusColors(green, green2);
                    imgCopy2.Data[x, y, 2] = MinusColors(red, red2);
                }

            imageBox2.Image = imgCopy2;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> imgCopy = sourceImage.CopyBlank();
            Image<Bgr, byte> imgCopy2 = sourceImage2.CopyBlank();
            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    byte red = sourceImage.Data[x, y, 2];
                    byte green = sourceImage.Data[x, y, 1];
                    byte blue = sourceImage.Data[x, y, 0];

                    byte red2 = sourceImage2.Data[x, y, 2];
                    byte green2 = sourceImage2.Data[x, y, 1];
                    byte blue2 = sourceImage2.Data[x, y, 0];

                    imgCopy2.Data[x, y, 0] = MultiplyColors(blue, blue2);
                    imgCopy2.Data[x, y, 1] = MultiplyColors(green, green2);
                    imgCopy2.Data[x, y, 2] = MultiplyColors(red, red2);
                }

            imageBox2.Image = imgCopy2;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> bgrImage = sourceImage.Copy();
            //imageBox1.Image = bgrImage.Resize(640, 480, Inter.Linear);
            var hsvImage = bgrImage.Convert<Hsv, byte>();
            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                { 
                    hsvImage.Data[x, y, 0] += 50;
                    hsvImage.Data[x, y, 1] += 50;
                    hsvImage.Data[x, y, 2] += 50;
                }

            imageBox2.Image = hsvImage;
        }

        int step = 1;

        private void button12_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> img;
            if (bufer1 != null)
            {
                img = bufer1.Copy();
            }
            else
            {
                img = sourceImage.Copy();
               // Image<Bgr, byte> blurImg = img.CopyBlank();
            }
           // Image<Bgr, byte> img = sourceImage.Copy();
            Image<Bgr, byte> blurImg = img.CopyBlank();

/*            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    if (x > 0 && y > 0 && x < (sourceImage.Width - 1) && y < (sourceImage.Height - 1))
                    {
                        img.Data[x, y, 0] = average(0, x, y);
                        img.Data[x, y, 1] = average(1, x, y);
                        img.Data[x, y, 2] = average(2, x, y);
                    }
                    y += (step - 1);
                }
                x += (step - 1);
            }*/

            CvInvoke.Blur(img, blurImg, new Size(trackBar3.Value, trackBar3.Value), new Point(-1, -1));

            imageBox2.Image = blurImg;
            bufer2 = blurImg;
        }

        private byte average(int channel, int x, int y)
        {
            int size = 3;
            List<int> window = new List<int>();
            
            for (int i = 0; i<size; i++)
            {
                for (int j = 0; j<size; j++)
                {
                    window.Add(sourceImage.Data[x - 1, y - 1, channel]);
                    window.Add(sourceImage.Data[x - 1, y, channel]);
                    window.Add(sourceImage.Data[x - 1, y + 1, channel]);
                    window.Add(sourceImage.Data[x, y - 1, channel]);
                    window.Add(sourceImage.Data[x, y, channel]);
                    window.Add(sourceImage.Data[x, y + 1, channel]);
                    window.Add(sourceImage.Data[x + 1, y - 1, channel]);
                    window.Add(sourceImage.Data[x + 1, y, channel]);
                    window.Add(sourceImage.Data[x + 1, y + 1, channel]);

                    //window.Add(sourceImage.Data[x]);
                }
            }
            window.Sort();

            return (byte)window[4];
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img = sourceImage.Convert<Gray, byte>();
            Image<Gray, byte> resImage = sourceImage.CopyBlank().Convert<Gray, byte>();

            List<int> Matrix = new List<int>() { -1, -1, -1, -1, trackBar4.Value, -1, -1, -1, -1 };
            for (int y = 1; y < img.Height-1; y++)
            {
                for(int x = 1; x < img.Width-1; x++)
                {
                    List<int> pixels = new List<int>();
                    int i = 0;

                    //Первый

                    for (int k1 = -1; k1 < 2; k1++)
                    {
                        for (int k2 = -1; k2 < 2; k2++)
                        {
                            pixels.Add(img.Data[y + k1, x + k2, 0] * Matrix[i]);
                            i++;
                        }
                    }
                    resImage.Data[y, x, 0] = (byte)(pixels.Sum() / Matrix.Sum());
                    //if (Matrix.Sum() != 0)
                    //{
                    //    resImage.Data[y, x, 0] = (byte)(pixels.Sum() / Matrix.Sum());
                    //}
                    //else
                    //{
                    //    resImage.Data[y, x, 0] = (byte)(pixels.Sum());
                    //}
                    //i = 0;
                }
            }

            imageBox2.Image = resImage;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img = sourceImage.Convert<Gray, byte>();
            Image<Gray, byte> resImage = sourceImage.CopyBlank().Convert<Gray, byte>();

            //List<int> Matrix = new List<int>() { -4, -2, 0, -2, 1, 2, 0, 2, 4 };
            List<int> Matrix = new List<int>() { -4, -2, 0, -2, 1, 2, 0, 2, 4 };

            for (int y = 1; y < img.Height - 1; y++)
            {
                for (int x = 1; x < img.Width - 1; x++)
                {
                    List<int> pixels = new List<int>();
                    int i = 0;

                    //Первый

                    for (int k1 = -1; k1 < 2; k1++)
                    {
                        for (int k2 = -1; k2 < 2; k2++)
                        {
                            pixels.Add(img.Data[y + k1, x + k2, 0] * Matrix[i]);
                            i++;
                        }
                    }
                    resImage.Data[y, x, 0] = (byte)(pixels.Sum() / Matrix.Sum());
                    //if (Matrix.Sum() != 0)
                    //{
                    //    resImage.Data[y, x, 0] = (byte)(pixels.Sum() / Matrix.Sum());
                    //}
                    //else
                    //{
                    //    resImage.Data[y, x, 0] = (byte)(pixels.Sum());
                    //}
                    //i = 0;
                }
            }

            imageBox2.Image = resImage;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //Яркость-Контраст
            Image<Bgr, byte> imgCopy = sourceImage.CopyBlank();
            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    byte red = sourceImage.Data[x, y, 2];
                    byte green = sourceImage.Data[x, y, 1];
                    byte blue = sourceImage.Data[x, y, 0];

                    byte redRes = Convert.ToByte(Math.Min((red * contrast + brightness), 255));
                    byte greenRes = Convert.ToByte(Math.Min((green * contrast + brightness), 255));
                    byte blueRes = Convert.ToByte(Math.Min((blue * contrast + brightness), 255));

                    imgCopy.Data[x, y, 0] = blueRes;
                    imgCopy.Data[x, y, 1] = greenRes;
                    imgCopy.Data[x, y, 2] = redRes;
                }
            bufer1 = imgCopy;

            Image<Bgr, byte> blurImg = bufer1.CopyBlank();

            CvInvoke.Blur(bufer1, blurImg, new Size(trackBar3.Value, trackBar3.Value), new Point(-1, -1));

            bufer2 = blurImg;

             imgCopy = bufer2.Resize(736, 736, Inter.Linear);
            Image<Bgr, byte> imgCopy2 = sourceImage2.CopyBlank();

            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    byte red = imgCopy.Data[x, y, 2];
                    byte green = imgCopy.Data[x, y, 1];
                    byte blue = imgCopy.Data[x, y, 0];

                    //byte red = sourceImage.Data[x, y, 2];
                    //byte green = sourceImage.Data[x, y, 1];
                    //byte blue = sourceImage.Data[x, y, 0];

                    byte red2 = sourceImage2.Data[x, y, 2];
                    byte green2 = sourceImage2.Data[x, y, 1];
                    byte blue2 = sourceImage2.Data[x, y, 0];

                    imgCopy2.Data[x, y, 0] = AddColors(blue, blue2);
                    imgCopy2.Data[x, y, 1] = AddColors(green, green2);
                    imgCopy2.Data[x, y, 2] = AddColors(red, red2);
                }
            bufer3 = imgCopy2;
            imageBox2.Image = bufer3;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> grayImage = sourceImage.Convert<Gray, byte>();

            var tempImage = grayImage.PyrDown();
            var destImage = tempImage.PyrUp();

            VectorOfMat vm = new VectorOfMat();
            vm.Push(sourceImage.Split()[0]); vm.Push(sourceImage.Split()[1]); vm.Push(sourceImage.Split()[2]);
            CvInvoke.Merge(vm, destImage);

            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                    grayImage.Data[y, x, 0] = Convert.ToByte(0.299 * sourceImage.Data[y, x, 2] + 0.587 * sourceImage.Data[y, x, 1] + 0.114 * sourceImage.Data[y, x, 0]);

            //imageBox3.Image = grayImage;
            // bufer = imageBox3.Image;
            //bufer3 = grayImage;

            Image<Bgr, byte> blurImg = sourceImage.CopyBlank();

            CvInvoke.Blur(sourceImage, blurImg, new Size(trackBar3.Value, trackBar3.Value), new Point(-1, -1));

            //bufer2g = blurImg.Convert<Gray, byte>();

            var edges = blurImg.Convert<Gray, byte>();
            edges = edges.ThresholdAdaptive(new Gray(100), AdaptiveThresholdType.MeanC, 
                ThresholdType.Binary, 3, new Gray(0.03*trackBar6.Value));

            //Image<Bgr, byte> edges2 = edges.Convert<Bgr, byte>();
            //bufer3 = edges2;

            //imageBox2.Image = bufer3;

            //Image<Bgr, byte> edges = sourceImage.CopyBlank();
            Image<Bgr, byte> edges2 = edges.Convert<Bgr, byte>();
            Image<Bgr, byte> imgCopy2 = sourceImage.CopyBlank();
            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    byte red = sourceImage.Data[x, y, 2];
                    byte green = sourceImage.Data[x, y, 1];
                    byte blue = sourceImage.Data[x, y, 0];

                    byte red2 = edges2.Data[x, y, 2];
                    byte green2 = edges2.Data[x, y, 1];
                    byte blue2 = edges2.Data[x, y, 0];

                    imgCopy2.Data[x, y, 0] = AddColors(blue, blue2);
                    //imgCopy2.Data[x,y,1] = sourceImage.Data[x, y, 1];
                    //imgCopy2.Data[x,y,2] = sourceImage.Data[x, y, 2];

                    imgCopy2.Data[x, y, 1] = AddColors(green, green2);
                    imgCopy2.Data[x, y, 2] = AddColors(red, red2);

                    //for (int j = 0; j < sourceImage.Width; j++)
                    //    for (int g = 0; g < sourceImage.Height; g++)
                    //        imgCopy2.Data[g, j, 0] = Convert.ToByte(0.299 * 
                    //            sourceImage.Data[g, j, 2] + 0.587 * 
                    //            sourceImage.Data[g, j, 1] + 0.114 * 
                    //            sourceImage.Data[g, j, 0]);

                }

            //for (int j = 0; j < sourceImage.Width; j++)
            //    for (int g = 0; g < sourceImage.Height; g++)
            //        imgCopy2.Data[g, j, 0] = Convert.ToByte(0.299 *
            //            sourceImage.Data[g, j, 2] + 0.587 *
            //            sourceImage.Data[g, j, 1] + 0.114 *
            //            sourceImage.Data[g, j, 0]);

            imageBox2.Image = imgCopy2;
        }
    }
}
