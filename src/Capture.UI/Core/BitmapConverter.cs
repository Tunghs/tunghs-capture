using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Capture.UI.Core
{
    internal class BitmapConverter
    {
        public BitmapImage BitmapToBitmapImage(Bitmap bmp)
        {
            BitmapImage bitmapimage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
    }
}
