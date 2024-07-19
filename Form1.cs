using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlumClickWinForm
{
    public partial class BlumClickForm : Form
    {
        private Bot _bot;
        private ToolTip _toolTipStart = new ToolTip();
        public BlumClickForm()
        {
            InitializeComponent();
            _bot = new Bot(this);
            Start();
        }

        private void Start()
        {
            Task.Run(async () => { await _bot.Run(); });
        }
        public void SetFormScale(int width, int height)
        {
            Width = width;
            Height = height + 240;
        }
        private void buttonRun_Click(object sender, System.EventArgs e)
        {
            _bot.SetActive(true);
            _bot.SetAutoStart(checkBoxAutoRun.Checked);
        }

        private void trackBarWidth_Scroll(object sender, System.EventArgs e)
        {
            ImageProcess.SetWidthScreenshot(trackBarWidth.Value);
        }
        private void trackBarHeight_Scroll(object sender, System.EventArgs e)
        {
            ImageProcess.SetHeightScreenshot(trackBarHeight.Value);
        }

        private void buttonHelp_Click(object sender, System.EventArgs e)
        {
            _toolTipStart.Show(
                "Нажмите CTRL для остановки бота." +
                "\nПолзунки служат для изменения параметров захвата изображения." +
                "\nСначала разместите окно BLum в центра экрана для корректной работы." +
                "\nЗатем нажмите кнопку Play в Blum. Далее нажмите кнопку Start в программе." +
                "\nGame Auto Run отвечает за автоматическое нажатие кнопки Play конце игры в Blum." +
                "\n\nЧем больше область тем меньше производительность!!!",
                buttonRun);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://t.me/CoderWorker");
        }

        private void checkBoxAutoRun_CheckedChanged(object sender, System.EventArgs e)
        {
            _bot.SetAutoStart(checkBoxAutoRun.Checked);
        }
    }
}
