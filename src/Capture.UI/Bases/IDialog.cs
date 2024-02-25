using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capture.UI.Bases
{
    public interface IDialog
    {
        string Title { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        object DataContext { get; set; }
        bool Activate();

        void Show();

        bool? ShowDialog();

        void Close();

        Action CloseCallback { get; set; }
    }
}
