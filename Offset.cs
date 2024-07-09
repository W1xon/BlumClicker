using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlumClickWinForm
{
    internal class Offset
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public Rectangle ScreenBounds { get; }

        private int _scalePercentage;
        private bool _isScaledUp;
        private System.Timers.Timer _timerIce;

        public int[] baseOffset;
        public Offset()
        {
            ScreenBounds = Screen.PrimaryScreen.Bounds;
            TimerInitialize();
            SetOffset();
        }

        private void TimerInitialize()
        {
            _timerIce = new System.Timers.Timer();
            _timerIce.Interval = 2500;
            _timerIce.Elapsed += timerIce_Tick;
        }
        public void SetOffset()
        {
            _scalePercentage = 30;
            int percent = 40;
            Left = ScreenBounds.Width * percent / 100;
            Top = ScreenBounds.Height * percent / 100;
            Right = ScreenBounds.Width * percent / 100;
            Bottom = ScreenBounds.Height * percent / 100;
            SaveBaseOffset();
        }
        public void SaveBaseOffset()
        {
            baseOffset = new int[4] { Left, Top, Right, Bottom };
        }
        public void TemporaryScaleUp()
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
            Top += baseOffset[1] * percentage / 100;
            Bottom += baseOffset[1] * percentage / 100;
        }
        private void timerIce_Tick(object sender, EventArgs e)
        {
            ScaleDown();
            _timerIce.Stop();
        }
    }
}
