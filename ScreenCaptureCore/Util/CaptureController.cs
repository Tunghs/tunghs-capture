using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ScreenCaptureCore.Util
{
    public class CaptureController
    {
        // 클립보드에 저장
        public void SaveToClipboard(int width, int height, int x, int y)
        {
            BitmapImage ClipImage;
            using (Bitmap bmp = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(x, y, 0, 0, bmp.Size);
                    BitmapController bitmapController = new BitmapController();
                    ClipImage = bitmapController.BitmapToImageSource(bmp);

                    Clipboard.SetImage(ClipImage);
                }
            }
        }
            

        // 경로에 저장
        public void SaveToPath(int width, int height, int x, int y, string savePath)
        {
            using (Bitmap bmp = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    
                    g.CopyFromScreen(x, y, 0, 0, bmp.Size);

                    if (!Directory.Exists(savePath))
                    {
                        System.Windows.Forms.MessageBox.Show("해당 경로가 존재하지 않습니다.");
                        return;
                    }


                    string filename = $"Capture_{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.png";
                    string filePath = Path.Combine(savePath, filename);
                    while (true)
                    {
                        filePath = Path.Combine(savePath, filename);
                        int index = 0;
                        if (File.Exists(filePath))
                        {
                            filename = $"Capture_{DateTime.Now.ToString("yyyyMMdd-HHmmss")}({index}).png";
                            index++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    bmp.Save(filePath);
                }
            }
        } 
    }
}
