using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ScreenCaptureCore.Util
{
    public class Capture
    {
        public void SaveClipboard(int x, int y, int width, int height)
        {
            using (Bitmap bmp = GetCaptureBitmap(x, y, width, height))
            {
                var img = BitmapToBitmapImage(bmp);
                Clipboard.SetImage(img);
            }
        }

        public void SaveFile(int x, int y, int width, int height, string path)
        {
            using(Bitmap bmp = GetCaptureBitmap(x, y, width, height))
            {
                if (!Directory.Exists(path))
                    return;

                var saveFilePath = GetSvaeFilePath(path);
                bmp.Save(saveFilePath);
            }
        }

        #region Private Method

        private Bitmap GetCaptureBitmap(int x, int y, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(x, y, 0, 0, bmp.Size);
                return bmp;
            }
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bmp)
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

        private string GetSvaeFilePath(string dirPath)
        {
            string filePath = $@"{dirPath}\{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
            string avaibleFilePath = filePath;
            int index = 0;
            while (true)
            {
                if (!File.Exists($"{avaibleFilePath}.png"))
                    break;

                index++;
                avaibleFilePath = $"{filePath}_{index}";
            }
            return $"{avaibleFilePath}.png";
        }

        #endregion
    }
}
