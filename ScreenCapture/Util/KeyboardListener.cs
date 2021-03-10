using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ScreenCapture.ViewModel;

namespace ScreenCapture.Util
{
    public class KeyboardListener : IDisposable
    {
        private static IntPtr _HookId = IntPtr.Zero;
        // 참조 카운트 유지
        private static InterceptKeys.LowLevelKeyboardProc _LowLevelKeyboardProc;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                //ScreenCaptureViewModel screenCaptureViewModel = new ScreenCaptureViewModel();
                //screenCaptureViewModel.MatchCaptureSetting();
                if (KeyboardDown != null)
                {
                    KeyboardDown();
                }

                return HookCallbackInner(nCode, wParam, lParam);
            }
            catch
            {
                Console.WriteLine("There was some error somewhere...");
            }
            return InterceptKeys.CallNextHookEx(_HookId, nCode, wParam, lParam);
        }

        private IntPtr HookCallbackInner(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)InterceptKeys.WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    if (KeyDown != null)
                    {
                        KeyDown(this, new RawKeyEventArgs(vkCode, false));
                    }
                }
                else if (wParam == (IntPtr)InterceptKeys.WM_KEYUP)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    if (KeyUp != null)
                    {
                        KeyUp(this, new RawKeyEventArgs(vkCode, false));
                    }
                }
            }
            return InterceptKeys.CallNextHookEx(_HookId, nCode, wParam, lParam);
        }

        public event RawKeyEventHandler KeyDown;
        public event RawKeyEventHandler KeyUp;
        public event KeyboardDownHandler KeyboardDown;


        public KeyboardListener()
        {
            


            // Managed 환경의 delegate 인스턴스를 Native에 전달해서 GC가 구동된 이후 delegate 인스턴스가 정리되어버리기 때문에 
            //이후에 native에서 삭제된 인스턴스의 delegate 값으로 호출하게 되면 "CallbackOnCollectedDelegate" 오류가 생긴다
            // 따라서 GC에 의해서 인스턴스가 정리되지 않도록 참조 포인터를 하나라도 유지하는 방법이 있으며, 이를 위해 코드 상단에 타입 멤버로 값을 가지고 있으면 된다.
            _LowLevelKeyboardProc = (InterceptKeys.LowLevelKeyboardProc)HookCallback;
            _HookId = InterceptKeys.SetHook(_LowLevelKeyboardProc);
        }

        ~KeyboardListener()
        {
            Dispose();
        }

        #region IDisposable Members
        public void Dispose()
        {
            InterceptKeys.UnhookWindowsHookEx(_HookId);
        }
        #endregion
    }

    internal static class InterceptKeys
    {
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static int WH_KEYBOARD_LL = 13;
        public static int WM_KEYDOWN = 0x0100;
        public static int WM_KEYUP = 0x0101;

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }

    public class RawKeyEventArgs : EventArgs
    {
        public int VKCode;
        public Key Key;
        public bool IsSysKey;

        public RawKeyEventArgs(int VKCode, bool isSysKey)
        {
            this.VKCode = VKCode;
            this.IsSysKey = isSysKey;
            this.Key = System.Windows.Input.KeyInterop.KeyFromVirtualKey(VKCode);
        }
    }

    public delegate void RawKeyEventHandler(object sender, RawKeyEventArgs args);
    public delegate void KeyboardDownHandler();
}
