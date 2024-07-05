using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlumClickWinForm
{
    internal class ImageProcess
    {
        private int LeftOffset = 760, TopOffset = 450, RightOffset = 770, BottomOffset = 450;

        private bool isScaleUp;
        private System.Timers.Timer timerIce;
        //Это цвета Зеленых штук
        private HashSet<int> targetGreenColors = new HashSet<int>
        {
        	 //цвет до обновления
            //ColorToInt(230, 255, 145),
            //ColorToInt(174, 254, 119),
            //ColorToInt(120, 250, 35),
            //ColorToInt(205, 220, 0),
            //ColorToInt(177, 255, 122),
            //ColorToInt(82, 225, 18),
            //ColorToInt(110, 229, 49),
            //ColorToInt(141, 246, 69),
            //ColorToInt(63, 219, 0),
            //ColorToInt(211, 254, 132),
            //ColorToInt(163, 237, 164),
            //ColorToInt(100, 218, 101),
            //ColorToInt(168, 221, 98),
            //ColorToInt(188, 236, 176),
            //ColorToInt(57, 214, 18),
            //ColorToInt(120, 249, 50),
            //ColorToInt(170, 236, 138),
            //ColorToInt(229, 254, 144),
            //ColorToInt(98, 217, 86),
            //ColorToInt(172, 252, 117),
            //ColorToInt(51, 209, 38),
            
            //цвет после обновления
            ColorToInt(255, 0, 198),
            ColorToInt(253, 1, 205),
            ColorToInt(255, 0, 198),
            ColorToInt(254, 0, 197),
            ColorToInt(255, 2, 200),
            ColorToInt(254, 1, 194),
            ColorToInt(168, 169, 167),
            ColorToInt(249, 254, 255),
            ColorToInt(42, 32, 27),
            ColorToInt(117, 115, 113),
            ColorToInt(89, 84, 70),
            ColorToInt(143, 144, 137),
            ColorToInt(146, 146, 139),
            ColorToInt(169, 165, 160),
            ColorToInt(159, 159, 159),
            ColorToInt(132, 132, 131),
            ColorToInt(180, 180, 178),
            ColorToInt(77, 75, 71),
            ColorToInt(105, 103, 95),
            ColorToInt(84, 79, 68),
            ColorToInt(146, 144, 141),
            ColorToInt(76, 74, 67),
            ColorToInt(150, 146, 141),
            ColorToInt(86, 84, 79),
            ColorToInt(101, 94, 85),
            ColorToInt(85, 91, 90),
            ColorToInt(53, 48, 37),
            ColorToInt(82, 81, 79),
            ColorToInt(102, 102, 101),
            ColorToInt(103, 101, 97),
            ColorToInt(128, 123, 118),
            ColorToInt(169, 165, 160),
            ColorToInt(92, 92, 92),
            ColorToInt(63, 60, 54),
            ColorToInt(94, 86, 80),
            ColorToInt(174, 172, 167),
            ColorToInt(114, 113, 105), 
            ColorToInt(120, 121, 118),
            ColorToInt(53, 49, 31),
            ColorToInt(216, 30, 164),
            ColorToInt(225, 9, 152),
            ColorToInt(200, 43, 154),
            ColorToInt(153, 34, 76),
        };
        
        //Это цвета льдинок
        private HashSet<int> targetIceColors = new HashSet<int>
        {
        	 //цвет до обновления
            //ColorToInt(85, 160, 220),
            //ColorToInt(130, 220, 233),
            //ColorToInt(85, 159, 220),
            //ColorToInt(167, 251, 242),
            //ColorToInt(85, 204, 220),
            
            //цвет после обовления
            ColorToInt(35, 173, 228),
            ColorToInt(27, 56, 98),
            ColorToInt(24, 95, 161),
            ColorToInt(9, 24, 58),
            ColorToInt(45, 149, 213),
            ColorToInt(31, 99, 162),
            ColorToInt(61, 91, 116),
            ColorToInt(63, 145, 211),
            ColorToInt(45, 186, 241),
            ColorToInt(72, 160, 211),
            ColorToInt(81, 165, 224),
        };

        public ImageProcess()
        {
            timerIce = new System.Timers.Timer();
            timerIce.Interval = 2500;
            timerIce.Elapsed += timerIce_Tick;
        }
        private static int ColorToInt(int r, int g, int b)
        {
            return (r << 16) | (g << 8) | b;
        }
        public Bitmap Screenshot()
        {
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;

            Rectangle captureRectangle = new Rectangle
                (
                LeftOffset,
                TopOffset,
                screenBounds.Width - LeftOffset - RightOffset,
                screenBounds.Height - TopOffset - BottomOffset
                );

            Bitmap img = new Bitmap(captureRectangle.Width, captureRectangle.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.CopyFromScreen(captureRectangle.Location, Point.Empty, img.Size);
            }
            return img;
        }
        public async Task DetectedPixel(Bitmap img)
        {
            int width = img.Width;
            int height = img.Height;

            BlumClick.SetFormScale(width, height);

            List<Point> points = new List<Point>();
            BitmapData bitmapData = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int bytesPerPixel = Image.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirsPixel = bitmapData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(ptrFirsPixel, pixels, 0, pixels.Length);
            img.UnlockBits(bitmapData);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int byteIndex = y * bitmapData.Stride + x * bytesPerPixel;
                    byte b = pixels[byteIndex];
                    byte g = pixels[byteIndex + 1];
                    byte r = pixels[byteIndex + 2];

                    int pixelColor = ColorToInt(r, g, b);

                    if (targetIceColors.Contains(pixelColor))
                    {
                        MouseSimulator.MouseMoveToPoint(new Point(x + LeftOffset, y + TopOffset));
                        TemporaryScaleUp();
                        return;
                    }
                    if (targetGreenColors.Contains(pixelColor))
                    {
                        MouseSimulator.MouseMoveToPoint(new Point(x + LeftOffset, y + TopOffset));
                        return;
                    }
                }
            }
        }
        private void timerIce_Tick(object sender, EventArgs e)
        {
            ScaleDown();
            timerIce.Stop();
        }
        private void TemporaryScaleUp()
        {
            if (isScaleUp) return;
            TopOffset -= 250;
            BottomOffset -= 150;
            timerIce.Start();
            isScaleUp = true;
        }
        private void ScaleDown()
        {
            TopOffset += 250;
            BottomOffset += 150;
            isScaleUp = false;
        }

    }
}
