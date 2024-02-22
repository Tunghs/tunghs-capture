using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ScreenCaptureCore.Util
{
    public class Capture
    {
        private double dpiScale;
        private int monitorWidth;
        private int monitorHeight;

        public Capture(double dpiScale, int monitorWidth, int monitorHeight)
        {
            this.dpiScale = dpiScale;
            this.monitorWidth = monitorWidth;
            this.monitorHeight = monitorHeight; 
        }

        public void SaveClipboard(int x, int y, int width, int height)
        {
            using (Bitmap bmp = GetCaptureBitmap(x, y, width, height))
            {
                var img = BitmapToBitmapImage(bmp);
                System.Windows.Clipboard.SetImage(img);
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
            ApplyDPI(ref x, ref y, ref width, ref height);
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(x, y, 0, 0, bmp.Size);
                return bmp;
            }
        }

        private void ApplyDPI(ref int x, ref int y, ref int width, ref int height)
        {
            x = (int)(x * dpiScale);
            y = (int)(y * dpiScale);
            width = (int)(width * dpiScale);
            height = (int)(height * dpiScale);
        }

        private Bitmap GetCaptureBitmap2(int x, int y, int cropWidth, int cropHeight)
        {
            var width = SystemParameters.PrimaryScreenWidth;
            var height = SystemParameters.PrimaryScreenHeight;
            System.Drawing.Size resize = new System.Drawing.Size((int)width, (int)height);
            Bitmap bmp;

            using (Bitmap curBmp = new Bitmap(monitorWidth, monitorHeight))
            using (Graphics g = Graphics.FromImage(curBmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, curBmp.Size);
                bmp = new Bitmap(curBmp, resize);

                // curBmp.Save(@"D:\b1.png");
                // bmp.Save(@"D:\b2.png");
            }
            var cropBmp = bmp.Clone(new Rectangle(x, y, cropWidth, cropHeight), System.Drawing.Imaging.PixelFormat.DontCare);

            return cropBmp;
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
            string saveFilePath = filePath;
            int index = 0;
            while (true)
            {
                if (!File.Exists($"{saveFilePath}.png"))
                    break;

                saveFilePath = $"{filePath}_{index}";
                index++;
            }
            return $"{saveFilePath}.png";
        }

        #endregion
    }
}
