using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenCaptureCore.Util
{
    public class ManualKeyboard
    {
        [DllImport("user32.dll")]
        static extern ushort GetAsynKeyState(Key vKey);

        public static bool IsKeyDown(Key key)
        {
            return 0 != (GetAsynKeyState(key) & 0x8000);
        }
    }
}
