using Capture.UI.Models;
using Capture.UI.Utils;

using System.Configuration;
using System.Data;
using System.Windows;

namespace Capture.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields
        private const string _APPLICATION_NAME_ = "Tunghs Capture";
        private static readonly string _title = "Tunghs Capture";
        #endregion

        #region Properties
        public static string Title
        {
            get { return _title; }
        }
        #endregion

        public App()
        {
            new Bootstrapper();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (this.CheckDuplicateProcess())
            {
                MessageBox.Show("프로그램이 이미 실행중 입니다.", "Warning");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        ///// <summary>
        ///// 프로세스 중복 체크
        ///// 이미 실행 중이라면 해당 Window Activate 처리
        ///// </summary>
        ///// <returns></returns>
        private bool CheckDuplicateProcess()
        {
            if (!ProcessChecker.Do(_APPLICATION_NAME_))
            {
                return false;
            }

            string processName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            int currentProcess = System.Diagnostics.Process.GetCurrentProcess().Id;
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(processName);
            foreach (System.Diagnostics.Process process in processes)
            {
                if (currentProcess == process.Id)
                {
                    continue;
                }

                // find MainWindow Title
                IntPtr hwnd = ProcessChecker.FindWindow(null, _APPLICATION_NAME_);
                if (hwnd.ToInt32() > 0)
                {
                    //Activate it
                    ProcessChecker.SetForegroundWindow(hwnd);

                    WindowShowStyle command = ProcessChecker.IsIconicNative(hwnd) ? WindowShowStyle.Restore : WindowShowStyle.Show;
                    ProcessChecker.ShowWindow(hwnd, command);
                }
            }
            Current.Shutdown();

            return true;
        }
    }
}
