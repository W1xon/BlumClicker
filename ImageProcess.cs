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
        private Offset _offset;
        private Rectangle _screenBounds;
        private bool _isScaledUp;
        private int _scalePercentage;

        private System.Timers.Timer _timerIce;

        public ImageProcess()
        {
            TimerInitialize();
            _screenBounds = Screen.PrimaryScreen.Bounds;
            _scalePercentage = 30;
            SetOffset();
        }
        public Rectangle GetScreenBounds()
        {
            return _screenBounds;
        }
        public Bitmap Screenshot()
        {
            Rectangle captureRectangle = CreateScreeenRectangle();

            Bitmap img = new Bitmap(captureRectangle.Width, captureRectangle.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.CopyFromScreen(captureRectangle.Location, Point.Empty, img.Size);
            }
            return img;
        }
        private Rectangle CreateScreeenRectangle()
        {
            int x = _offset.Left;
            int y = _offset.Top;
            int width = _screenBounds.Width - _offset.Left - _offset.Right;
            int height = _screenBounds.Height - _offset.Top - _offset.Bottom;
            if (width < 10) width = 1;
            if (height < 10) height = 1;
            return new Rectangle(x, y, width, height);
        }
        public async Task DetectedPixel(Bitmap img)
        {
            int width = img.Width;
            int height = img.Height;

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

                    int pixelColor = LibraryColor.ColorToInt(r, g, b);

                    if (LibraryColor.frostColors.Contains(pixelColor))
                    {
                        MouseSimulator.MouseMoveToPoint(new Point(x + _offset.Left, y + _offset.Top));
                        TemporaryScaleUp();
                        return;
                    }
                    if (LibraryColor.itemColors.Contains(pixelColor))
                    {
                        MouseSimulator.MouseMoveToPoint(new Point(x + _offset.Left, y + _offset.Top));
                        return;
                    }
                }
            }
        }
        public void SetWidthScreenshot(int addOffset)
        {
            if (!CheckBounds(_offset.Left, _offset.Right, _screenBounds.Width, addOffset)) return;
            _offset.Left = _offset.baseOffset[0] - addOffset;
            _offset.Right = _offset.baseOffset[2] - addOffset;
        }
        public void SetHeightScreenshot(int addOffset)
        {
            if (!CheckBounds(_offset.Top, _offset.Bottom, _screenBounds.Height, addOffset)) return;
            _offset.Top = _offset.baseOffset[1] - addOffset;
            _offset.Bottom = _offset.baseOffset[3] - addOffset;
        }
        private bool CheckBounds(int oneOffset, int twoOffset, int maxSize, int addOffset)
        {
            if (oneOffset - addOffset <= 0 || twoOffset - addOffset <= 0) return false;
            if (maxSize - (oneOffset + addOffset) - (twoOffset + addOffset) <= 0) return false;
            return true;
        }
        private void TimerInitialize()
        {
            _timerIce = new System.Timers.Timer();
            _timerIce.Interval = 2500;
            _timerIce.Elapsed += timerIce_Tick;
        }
        private void SetOffset()
        {
            int percent = 40;
            _offset.Left = _screenBounds.Width * percent / 100;
            _offset.Top = _screenBounds.Height * percent / 100;
            _offset.Right = _screenBounds.Width * percent / 100;
            _offset.Bottom = _screenBounds.Height * percent / 100;
            _offset.SaveBaseOffset();
        }
        private void timerIce_Tick(object sender, EventArgs e)
        {
            ScaleDown();
            _timerIce.Stop();
        }
        private void TemporaryScaleUp()
        {
            if (_isScaledUp) return;
            SetScaleOffset(-_scalePercentage);
            _timerIce.Start();
            _isScaledUp = true;
        }
        private void ScaleDown()
        {
            SetScaleOffset(_scalePercentage);
            _isScaledUp = false;
        }

        private void SetScaleOffset(int percentage)
        {
            _offset.Top += _offset.baseOffset[1] * percentage / 100;
            _offset.Bottom += _offset.baseOffset[1] * percentage / 100;
        }
    }
}
