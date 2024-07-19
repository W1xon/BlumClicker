using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System;
using System.Windows.Forms;
namespace BlumClickWinForm
{
    internal class Bot
    {
        private BotAfk _botAfk;
        private bool _isActive = false;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private BlumClickForm _blumForm;
        public Bot(BlumClickForm form)
        {
            _blumForm = form;
            _botAfk = new BotAfk(this, _blumForm);
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

                         Bitmap screen = DisplayImage(_blumForm.pictureBoxScreen);
                         _blumForm.Invoke((Action)(() => _blumForm.SetFormScale(screen.Width, screen.Height)));
                         if (!_isActive || _botAfk.clickNewGame) return;
                         await ImageProcess.DetectedPixel(screen);
                     }
                     finally
                     {
                         _semaphore.Release();
                     }
                 });
            }
        }

        public bool GetActive()
        {
            return _isActive;
        }
        public void SetActive(bool isActive)
        {
            _blumForm.buttonRun.Enabled = !isActive;
            _isActive = isActive;
            if (!isActive)
            {
                _botAfk.StopTimer();
            }
        }
        public void SetAutoStart(bool isStart)
        {
            _botAfk.isAutoStart = isStart;
            if (isStart && _isActive)
            {
                _botAfk.StartTimer();
            }
        }
        public Bitmap DisplayImage(PictureBox pictureBox)
        {
            Bitmap screen = ImageProcess.Screenshot();
            Bitmap picture = new Bitmap(screen);
            _blumForm.Invoke((Action)(() => pictureBox.Image = picture));
            return screen;
        }
    }
}
