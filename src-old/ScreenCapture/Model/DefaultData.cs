using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapture.Model
{
    public static class DefaultData
    {
        public static string BasePath { get; private set; }

        public static void Seed()
        {
            BasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }
    }
}
