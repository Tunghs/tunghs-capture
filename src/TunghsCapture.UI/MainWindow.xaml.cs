using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

using TunghsCapture.UI.Core;

using Wpf.Ui.Controls;

namespace TunghsCapture.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        private ScreenCapture _capture;
        private BitmapConverter _bitmapConverter;

        public MainWindow()
        {
            InitializeComponent();

            _capture = new ScreenCapture();
            _bitmapConverter = new BitmapConverter();
            // ShowTestUI();
        }

        private void ShowTestUI()
        {
            using (Bitmap sceen = _capture.Capture(1920, 0, 1920, 1080))
            {
                BitmapImage screenImg = _bitmapConverter.BitmapToBitmapImage(sceen);
                ScreenCaptureWindow wd = new ScreenCaptureWindow(screenImg);
                wd.WindowStyle = WindowStyle.None;
                wd.AllowsTransparency = true;
                wd.UseLayoutRounding = false;
                wd.Width = 1920;
                wd.Height = 1080;
                wd.Left = 1920;
                wd.Top = 0;
                wd.Show();

            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var screens = Screen.AllScreens;
            foreach (var screen in screens )
            {
                Console.WriteLine(screen.DeviceName);
                Console.WriteLine(screen.Bounds.Location);

                int Left = screen.Bounds.Left;
                int top = screen.Bounds.Top;
                int width  = screen.Bounds.Width;
                int height = screen.Bounds.Height;


                using (Bitmap sceen = _capture.Capture(Left, top, width, height))
                {
                    BitmapImage screenImg = _bitmapConverter.BitmapToBitmapImage(sceen);

                    ScreenCaptureWindow wd = new ScreenCaptureWindow(screenImg);
                    wd.Width = screen.Bounds.Width;
                    wd.Height = screen.Bounds.Height;
                    wd.Left = screen.Bounds.Left;
                    wd.Top = screen.Bounds.Top;

                    wd.Show();
                }
            }
        }
    }
}