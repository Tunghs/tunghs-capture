using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace TunghsCapture.UI.Core.DrawingStrokes
{
    internal abstract class DrawStrokeBase
    {
        public abstract void OnMouseDown(InkCanvas inkCanvas, MouseButtonEventArgs e);

        public abstract void OnMouseMove(InkCanvas inkCanvas, System.Windows.Input.MouseEventArgs e);

        public abstract void OnMouseRightClick(InkCanvas inkCanvas, MouseButtonEventArgs e);

        public StrokeBase StrokeResult { get; set; }
        public delegate void OnDrawPolyEventHandler();
        public event OnDrawPolyEventHandler OnDrawPolyEvent;

        protected virtual void OnDraw(EventArgs e)
        {
            if (OnDrawPolyEvent != null)
            {
                OnDrawPolyEvent();
            }
        }
    }
}
