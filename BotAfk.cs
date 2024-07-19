using System;
using System.Drawing;
using System.Threading;

namespace BlumClickWinForm
{
    internal class BotAfk
    {
        public bool isAutoStart;
        public bool clickNewGame;
        private System.Timers.Timer _timerNewGame;
        private System.Timers.Timer _timerForNewGame;
        private double _timeForNewGame = 50;
        private BlumClickForm _blumForm;
        private Bot _bot;
        public BotAfk(Bot bot, BlumClickForm blumForm)
        {
            _blumForm = blumForm;
            _bot = bot;
            TimerInitialize();
        }
        public void StartTimer()
        {
            _timerNewGame.Start();
            _timerForNewGame.Start();
        }
        public void StopTimer()
        {
            _timeForNewGame = 0;
            ShowTimeInfo();
            _timerNewGame.Stop();
            _timerForNewGame.Stop();
        }
        private void TimerInitialize()
        {
            _timerNewGame = new System.Timers.Timer();
            _timerForNewGame = new System.Timers.Timer();
            _timerForNewGame.Interval = 1000;
            _timerNewGame.Interval = 50000;
            _timerNewGame.Elapsed += _timerNewGame_Elapsed;
            _timerForNewGame.Elapsed += _timerNewGame_ElapsedTime;
        }
        private void _timerNewGame_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timerNewGame.Stop();
            _timerForNewGame.Stop();
            if (_bot.GetActive() && isAutoStart)
            {
                clickNewGame = true;
                MouseSimulator.MouseMoveToPoint(GetBlumStartPosition());
            }
            Thread.Sleep(100);
            clickNewGame = false;
            _timeForNewGame = 50;
            _timerForNewGame.Start();
            _timerNewGame.Start();
        }
        private void _timerNewGame_ElapsedTime(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timeForNewGame -= 1;
            ShowTimeInfo();
        }
        public void ShowTimeInfo()
        {
            _blumForm.Invoke((Action)(() =>
            {
                _blumForm.labelTime.Text = "Time to new Play: " + _timeForNewGame.ToString("0.0");
            }));
        }
        public Point GetBlumStartPosition()
        {
            Point position = new Point();
            position.X = ImageProcess.GetScreenBounds().Width / 2;
            position.Y = ((ImageProcess.GetScreenBounds().Height - 695) / 2) + 600;
            return position;
        }
    }
}
