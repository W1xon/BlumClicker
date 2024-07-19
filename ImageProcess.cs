using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace BlumClickWinForm
{

    internal static class ImageProcess
    {
        private static Offset _offset = new Offset();

        public static Rectangle GetScreenBounds()
        {
            return _offset.ScreenBounds;
        }
        public static Bitmap Screenshot()
        {
            Rectangle captureRectangle = CreateScreeenRectangle();

            Bitmap img = new Bitmap(captureRectangle.Width, captureRectangle.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.CopyFromScreen(captureRectangle.Location, Point.Empty, img.Size);
            }
            return img;
        }
        private static Rectangle CreateScreeenRectangle()
        {
            int x = _offset.Left;
            int y = _offset.Top;
            int width = _offset.ScreenBounds.Width - _offset.Left - _offset.Right;
            int height = _offset.ScreenBounds.Height - _offset.Top - _offset.Bottom;
            CheckWidth(ref width);
            CheckHeight(ref height);
            return new Rectangle(x, y, width, height);
        }
        public static async Task DetectedPixel(Bitmap img)
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
                        _offset.TemporaryScaleUp();
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
        public static  void SetWidthScreenshot(int addOffset)
        {
            _offset.Left = _offset.baseOffset[0] - addOffset;
            _offset.Right = _offset.baseOffset[2] - addOffset;
        }
        public static void SetHeightScreenshot(int addOffset)
        {
            _offset.Top = _offset.baseOffset[1] - addOffset;
            _offset.Bottom = _offset.baseOffset[3] - addOffset;
        }
        private static  void CheckWidth(ref int width)
        {
            if (width < 0)
                width = 1;
            if (width > _offset.ScreenBounds.Width)
                width = _offset.ScreenBounds.Width;
        }
        private static void CheckHeight(ref int height)
        {
            if (height < 0)
                height = 1;
            if (height > _offset.ScreenBounds.Height)
                height = _offset.ScreenBounds.Height;
        }
    }
}
