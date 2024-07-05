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

        public static BlumClick form1;
        public BlumClick()
        {
            InitializeComponent();
            form1 = this;
            imageProcess = new ImageProcess();

        }
        private async Task RunClicker()
        {
            while (!KeyControl.IsActiveCTRL())
            {
                await Task.Run(async () =>
                 {
                     semaphore.Wait();
                     try
                     {
                         Bitmap screen = imageProcess.Screenshot();
                         Bitmap picture = new Bitmap(screen);
                         pictureBoxScreen.Image = picture;
                         await imageProcess.DetectedPixel(screen);
                     }
                     finally
                     {
                         semaphore.Release();
                     }
                 });
            }
            buttonRun.Enabled = true;
            return;
        }
        public static void SetFormScale(int width, int height)
        {
            form1.Width = width;
            form1.Height = height + 80;
        }

        private void buttonRun_Click(object sender, System.EventArgs e)
        {
            buttonRun.Enabled = false;
            Task.Run(async () =>
            {
                await RunClicker();
            });
        }
    }
}
