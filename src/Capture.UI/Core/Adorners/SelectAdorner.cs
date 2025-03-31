using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;

namespace Capture.UI.Core.Adorners
{
    internal class SelectAdorner : Adorner
    {
        private Point? _StartPoint;
        private Point? _EndPoint;
        private Pen _Pen;
        private Brush _SelectBacground;
        private InkCanvas inkCanvas;

        public SelectAdorner(InkCanvas inkCanvas, Point? dragStartPoint)
            : base(inkCanvas)
        {
            this.inkCanvas = inkCanvas;
            this._StartPoint = dragStartPoint;
            this._EndPoint = Mouse.GetPosition(inkCanvas);

            this.inkCanvas.EditingMode = InkCanvasEditingMode.None;
            _Pen = new Pen(Brushes.LightSlateGray, 1);
            _Pen.DashStyle = new DashStyle(new double[] { 2 }, 1);
            _SelectBacground = (Brush)new BrushConverter().ConvertFrom("#32D3D3D3");
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.IsMouseCaptured)
                    this.CaptureMouse();

                _EndPoint = e.GetPosition(inkCanvas);
                UpdateSelection();
                this.InvalidateVisual();
            }
            else
            {
                if (this.IsMouseCaptured)
                    this.ReleaseMouseCapture();
            }
        }

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.IsMouseCaptured)
                this.ReleaseMouseCapture();

            // remove adorner layer
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.inkCanvas);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }
            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (this._StartPoint.HasValue && this._EndPoint.HasValue)
                dc.DrawRectangle(_SelectBacground, _Pen, new Rect(this._StartPoint.Value, this._EndPoint.Value));
        }

        /// <summary>
        /// Update Selected Strokes
        /// </summary>
        private void UpdateSelection()
        {
            Rect rubberBand = new Rect(_StartPoint.Value, _EndPoint.Value);
            StrokeCollection lstSelectedStrokes = new StrokeCollection();
            foreach (var item in inkCanvas.Strokes)
            {
                Rect itemRect = item.GetBounds();
                if (rubberBand.Contains(itemRect))
                {
                    lstSelectedStrokes.Add(item);
                }
            }

            inkCanvas.Select(lstSelectedStrokes);
            if (lstSelectedStrokes.Count == 0)
                inkCanvas.EditingMode = InkCanvasEditingMode.None;
        }
    }
}
