using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System;
using System.Windows.Forms;
namespace BlumClickWinForm
{
    internal class Bot
    {
        public bool _isAutoStart;
        private bool _isActive = false;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private ImageProcess _imageProcess;
        private BlumClickForm _blumForm;
        private System.Timers.Timer _timerNewGame;
        private System.Timers.Timer _timerForNewGame;
        private bool _clickNewGame;
        private double _timeForNewGame = 43;
        public Bot(BlumClickForm form)
        {
            _blumForm = form;
            _imageProcess = new ImageProcess();
            TimerInitialize();
        }

        private void TimerInitialize()
        {
            _timerNewGame = new System.Timers.Timer();
            _timerForNewGame = new System.Timers.Timer();
            _timerForNewGame.Interval = 1000;
            _timerNewGame.Interval = 43000;
            _timerNewGame.Elapsed += _timerNewGame_Elapsed;
            _timerForNewGame.Elapsed += _timerNewGame_ElapsedTime;
        }
        public void ShowTimeInfo()
        {
            _blumForm.Invoke((Action)(() =>
            {
                _blumForm.labelTime.Text = "Time to new Play: " + _timeForNewGame.ToString("0.0");
            }));
        }
        private void _timerNewGame_ElapsedTime(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timeForNewGame -= 1;
            ShowTimeInfo();
        }

        private void _timerNewGame_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timerNewGame.Stop();
            _timerForNewGame.Stop();
            if (_isActive && _isAutoStart)
            {
                _clickNewGame = true;
                MouseSimulator.MouseMoveToPoint(GetBlumStartPosition());
            }
            Thread.Sleep(100);
            _clickNewGame = false;
            _timeForNewGame = 43;
            _timerForNewGame.Start();
            _timerNewGame.Start();
        }

        public async Task Run()
        {
            while (true)
            {
                if (KeyControl.IsActiveCTRL())
                    SetActive(false);
                await Task.Run(async () =>
                 {
                     try
                     {
                         _semaphore.Wait();

                         Bitmap screen = ImageCapture(_blumForm.pictureBoxScreen);
                         _blumForm.Invoke((Action)(() => _blumForm.SetFormScale(screen.Width, screen.Height)));
                         if (!_isActive || _clickNewGame) return;
                         await _imageProcess.DetectedPixel(screen);
                     }
                     finally
                     {
                         _semaphore.Release();
                     }
                 });
            }
        }
        public ImageProcess GetImage() => _imageProcess;
        public void SetActive(bool isActive)
        {
            _blumForm.buttonRun.Enabled = !isActive;
            _isActive = isActive;
            if (!isActive)
            {
                _timerForNewGame.Stop();
                _timerNewGame.Stop();
            }
        }
        public void SetAutoStart(bool isStart)
        {
            _isAutoStart = isStart;
            if (_isAutoStart && _isActive)
            {
                _timeForNewGame = 43;
                _timerForNewGame.Start();
                _timerNewGame.Start();
            }
        }
        private Point GetBlumStartPosition()
        {
            Point position = new Point();
            position.X = _imageProcess.GetScreenBounds().Width / 2;
            position.Y = ((_imageProcess.GetScreenBounds().Height - 695) / 2) + 600;
            return position;
        }
        public Bitmap ImageCapture(PictureBox pictureBox)
        {
            Bitmap screen = _imageProcess.Screenshot();
            Bitmap picture = new Bitmap(screen);
            _blumForm.Invoke((Action)(() => pictureBox.Image = picture));
            return screen;
        }
    }
}
