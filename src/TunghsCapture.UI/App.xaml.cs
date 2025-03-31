using CommunityToolkit.Mvvm.DependencyInjection;

using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

using TunghsCapture.UI.Core;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TunghsCapture.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        [DllImport("Shcore.dll")]
        private static extern int GetDpiForMonitor(IntPtr hmonitor, int dpiType, out uint dpiX, out uint dpiY);

        [DllImport("User32.dll")]
        private static extern IntPtr MonitorFromPoint(System.Drawing.Point pt, uint dwFlags);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr lprcMonitor, IntPtr dwData);

        private const string _APPLICATION_NAME_ = "TunghsCapture";
        private const int MDT_EFFECTIVE_DPI = 0;
        private const uint MONITOR_DEFAULTTONEAREST = 2;

        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Screen mainScreen = Screen.PrimaryScreen;
            IntPtr hMonitor = MonitorFromPoint(mainScreen.Bounds.Location, MONITOR_DEFAULTTONEAREST);

            MainWindow window = new MainWindow();

            // DPI 가져오기
            if (GetDpiForMonitor(hMonitor, MDT_EFFECTIVE_DPI, out uint dpiX, out uint dpiY) == 0)
            {
                double scaleX = dpiX / 96.0;
                double scaleY = dpiY / 96.0;

                // 🎯 모니터의 좌표에 맞춰 창을 띄우기
                window.Left = mainScreen.Bounds.Left;
                window.Top = mainScreen.Bounds.Top;
                window.Width *= scaleX;
                window.Height *= scaleY;
            }

            window.Show();
        }
    }
}
