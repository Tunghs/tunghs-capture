using Capture.UI.Models;

using System.Runtime.InteropServices;

namespace Capture.UI.Utils
{
    public class ProcessChecker
    {
        private static Mutex _mutex;

        /// <summary>
        /// Application의 Instance 실행 여부
        /// </summary>
        /// <param name="processName">
        /// 
        /// </param>
        /// <returns>
        /// 실행중 = true / 실행중인 프로세스 없음 = false
        /// </returns>
        public static bool Do(string processName)
        {
            try
            {
                _mutex = new Mutex(false, processName);
            }
            catch (Exception ex)
            {
                return true;
            }

            try
            {
                if (!_mutex.WaitOne(0, false))
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }

            return false;
        }

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindowNative(string className, string windowName);

        //Import the SetForeground API to activate it
        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern IntPtr SetForegroundWindowNative(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern int ShowWindowNative(IntPtr hwnd, int iCmdShow);

        [DllImport("user32.dll", EntryPoint = "IsIconic")]
        public static extern bool IsIconicNative(IntPtr hwnd);

        /// <summary>
        /// 현재 활성화된 WindowHandle을 찾는다.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="windowName">Window Title.</param>
        /// <returns></returns>
        public static IntPtr FindWindow(string className, string windowName)
        {
            return FindWindowNative(className, windowName);
        }

        /// <summary>
        /// 해당 핸들을 가진 Window를 Foreground로 Activate 시킨다.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static IntPtr SetForegroundWindow(IntPtr hWnd)
        {
            return SetForegroundWindowNative(hWnd);
        }

        /// <summary>
        /// 해당 핸들의 Window가 minimize된 상태인지 확인한다.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static bool IsIconic(IntPtr hwnd)
        {
            return IsIconicNative(hwnd);
        }

        /// <summary>
        /// 해당 핸들의 Window를 Command 상태로 전환한다.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static int ShowWindow(IntPtr hwnd, WindowShowStyle command)
        {
            return ShowWindowNative(hwnd, (int)command);
        }
    }
}
