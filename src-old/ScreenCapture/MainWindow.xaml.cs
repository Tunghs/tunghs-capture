using MahApps.Metro.Controls;
using System.Windows.Threading;
using System.Windows.Forms;
using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Drawing;
using ScreenCapture.ViewModel;

namespace ScreenCapture
{
    public partial class MainWindow : MetroWindow
    {
        private const int Default_DPI = 96;
        public NotifyIcon Notify;
        public ScreenCaptureViewModel ScreenCaptureVM { get; set; } 

        public MainWindow()
        {
            InitializeComponent();
            ScreenCaptureVM = new ScreenCaptureViewModel();
            this.DataContext = ScreenCaptureVM;
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetDPI();
            InitNotify();
        }

        private void SetDPI()
        {
            PresentationSource source = PresentationSource.FromVisual(this);
            if (source == null) 
                return;

            double dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
            double dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            ScreenCaptureVM.SetMonitorInfo(dpiX / Default_DPI);
        }

        private void InitNotify()
        {
            try
            {
                ContextMenu menu = new ContextMenu();
                MenuItem item1 = new MenuItem();
                MenuItem item2 = new MenuItem();

                menu.MenuItems.Add(item1);
                menu.MenuItems.Add(item2);

                item1.Index = 0;
                item1.Text = "프로그램 종료";
                item1.Click += delegate (object click, EventArgs eClick)
                {
                    this.Close();
                };

                Notify = new NotifyIcon();
                Notify.Icon = ScreenCapture.Properties.Resources.ScreenCapture;
                Notify.Visible = true;
                Notify.DoubleClick += delegate (object senders, EventArgs ergs)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };

                Notify.ContextMenu = menu;
                Notify.Text = "ScreenCapture";

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState.Minimized.Equals(WindowState))
            {
                this.Hide();

                Notify.BalloonTipTitle = "ScreenCapture";
                Notify.BalloonTipText = "ScreenCapture";
                Notify.ShowBalloonTip(500);
            }
        }

        private void MetroWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
