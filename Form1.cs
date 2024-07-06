using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlumClickWinForm
{
    public partial class BlumClick : Form
    {
        private ImageProcess _imageProcess;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private bool _isClickerActive = false;

        private ToolTip _toolTipStart = new ToolTip();
        public BlumClick()
        {
            InitializeComponent();
            _imageProcess = new ImageProcess();

            Task.Run(async () =>
            {
                await RunClicker();
            });
        }
        private async Task RunClicker()
        {
            while (true)
            {
                if (KeyControl.IsActiveCTRL())
                {
                    buttonRun.Enabled = true;
                    _isClickerActive = false;
                }

                await Task.Run(async () =>
                 {
                     _semaphore.Wait();
                     try
                     {
                         Bitmap screen = ImageCapture();
                         SetFormScale(screen.Width, screen.Height);
                         if (!_isClickerActive) return;
                         await _imageProcess.DetectedPixel(screen);
                     }
                     finally
                     {
                         _semaphore.Release();
                     }
                 });
            }
        }
        private Bitmap ImageCapture()
        {
            Bitmap screen = _imageProcess.Screenshot();
            Bitmap picture = new Bitmap(screen);
            pictureBoxScreen.Image = picture;
            return screen;
        }
        public void SetFormScale(int width, int height)
        {
            Width = width;
            Height = height + 100;
        }

        private void buttonRun_Click(object sender, System.EventArgs e)
        {
            _isClickerActive = true;
            buttonRun.Enabled = false;
        }

        private void trackBarWidth_Scroll(object sender, System.EventArgs e)
        {
            _imageProcess.SetWidthScreenshot(trackBarWidth.Value);
        }

        private void trackBarHeight_Scroll(object sender, System.EventArgs e)
        {
            _imageProcess.SetHeightScreenshot(trackBarHeight.Value);
        }

        private void buttonHelp_Click(object sender, System.EventArgs e)
        {
            _toolTipStart.Show(
                "Нажмите CTRL для остановки бота." +
                "\nПолзунки служат для изменения параметров захвата изображения." +
                "\nСначала подгоните ширину под размер Blum" +
                "\nЗатем высоту видимости настройте по вашему желанию." +
                "\nРазместите окно Blum в центре этой области." +
                "\n\nЧем больше область тем меньше производительность!!!",
                buttonRun);
        }
    }
}
