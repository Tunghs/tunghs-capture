using Capture.UI.Core;

using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Wpf.Ui.Controls;

namespace Capture.UI
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
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var screens = Screen.AllScreens;
            foreach (var screen in screens)
            {
                Console.WriteLine(screen.DeviceName);
                Console.WriteLine(screen.Bounds.Location);

                int Left = screen.Bounds.Left;
                int top = screen.Bounds.Top;
                int width = screen.Bounds.Width;
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