using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlumClickWinForm
{
    public partial class BlumClick : Form
    {
        ImageProcess imageProcess;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        private bool isClickerActive = false;

        private ToolTip toolTipStart = new ToolTip();
        public BlumClick()
        {
            InitializeComponent();
            imageProcess = new ImageProcess();

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
                    isClickerActive = false;
                }

                await Task.Run(async () =>
                 {
                     semaphore.Wait();
                     try
                     {
                         Bitmap screen = ImageCapture();
                         SetFormScale(screen.Width, screen.Height);
                         if (!isClickerActive) return;
                         await imageProcess.DetectedPixel(screen);
                     }
                     finally
                     {
                         semaphore.Release();
                     }
                 });
            }
        }
        private Bitmap ImageCapture()
        {
            Bitmap screen = imageProcess.Screenshot();
            Bitmap picture = new Bitmap(screen);
            pictureBoxScreen.Image = picture;
            return screen;
        }
        public void SetFormScale(int width, int height)
        {
            Width = width;
            Height = height + 80;
        }

        private void buttonRun_Click(object sender, System.EventArgs e)
        {
            isClickerActive = true;
            buttonRun.Enabled = false;
        }

        private void buttonRun_MouseEnter(object sender, System.EventArgs e)
        {
            toolTipStart.Show("Press CTRL to STOP", buttonRun);
        }
    }
}
