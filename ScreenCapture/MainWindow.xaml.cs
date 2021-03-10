using MahApps.Metro.Controls;
using System.Windows.Threading;
using System.Windows.Forms;
using System;
using System.Windows;

namespace ScreenCapture
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private DispatcherTimer _Timer;
        public NotifyIcon Notify;
        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
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
            catch(Exception ex)
            {

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
