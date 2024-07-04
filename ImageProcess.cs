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

        private HashSet<int> targetGreenColors = new HashSet<int>
        {
            ColorToInt(230, 255, 145),
            ColorToInt(174, 254, 119),
            ColorToInt(120, 250, 35),
            ColorToInt(205, 220, 0),
            ColorToInt(177, 255, 122),
            ColorToInt(82, 225, 18),
            ColorToInt(110, 229, 49),
            ColorToInt(141, 246, 69),
            ColorToInt(63, 219, 0),
            ColorToInt(211, 254, 132),
            ColorToInt(163, 237, 164),
            ColorToInt(100, 218, 101),
            ColorToInt(168, 221, 98),
            ColorToInt(188, 236, 176),
            ColorToInt(57, 214, 18),
            ColorToInt(120, 249, 50),
            ColorToInt(170, 236, 138),
            ColorToInt(229, 254, 144),
            ColorToInt(98, 217, 86),
            ColorToInt(172, 252, 117),
            ColorToInt(51, 209, 38),
        };
        private HashSet<int> targetIceColors = new HashSet<int>
        {
            ColorToInt(85, 160, 220),
            ColorToInt(130, 220, 233),
            ColorToInt(85, 159, 220),
            ColorToInt(167, 251, 242),
            ColorToInt(85, 204, 220),
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
