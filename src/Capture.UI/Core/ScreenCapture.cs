using System.Drawing;

namespace Capture.UI.Core
{
    internal class ScreenCapture
    {
        public Bitmap Capture(int x, int y, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(x, y, 0, 0, bmp.Size);
                return bmp;
            }
        }
    }
}
