using Capture.UI.Core;

using System.Windows.Media.Imaging;

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

        private void DragBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
            //var screens = Screen.AllScreens;
            //foreach (var screen in screens)
            //{
            //    Console.WriteLine(screen.DeviceName);
            //    Console.WriteLine(screen.Bounds.Location);

            //    int Left = screen.Bounds.Left;
            //    int top = screen.Bounds.Top;
            //    int width = screen.Bounds.Width;
            //    int height = screen.Bounds.Height;

            //    using (Bitmap sceen = _capture.Capture(Left, top, width, height))
            //    {
            //        BitmapImage screenImg = _bitmapConverter.BitmapToBitmapImage(sceen);

            //        ScreenCaptureWindow wd = new ScreenCaptureWindow(screenImg);
            //        wd.Width = screen.Bounds.Width;
            //        wd.Height = screen.Bounds.Height;
            //        wd.Left = screen.Bounds.Left;
            //        wd.Top = screen.Bounds.Top;

            //        wd.Show();
            //    }
            //}
        }

        private void Button_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}