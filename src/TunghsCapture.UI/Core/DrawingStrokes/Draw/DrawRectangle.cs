using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Ink;
using System.Windows.Media;
using Point = System.Windows.Point;
using Brushes = System.Windows.Media.Brushes;
using TunghsCapture.UI.Core.DrawingStrokes.Strokes;

namespace TunghsCapture.UI.Core.DrawingStrokes.Draw
{
    internal class DrawRectangle : DrawStrokeBase
    {
        protected Point topLeft;
        protected Point bottomRight;

        public override void OnMouseDown(System.Windows.Controls.InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            StrokeResult = null;
            topLeft = e.GetPosition(inkCanvas);
        }

        public override void OnMouseMove(System.Windows.Controls.InkCanvas inkCanvas, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                bottomRight = e.GetPosition(inkCanvas);
                if (topLeft != bottomRight)
                {
                    StylusPointCollection pts = new StylusPointCollection();
                    GetRectangle(pts, (s) =>
                    {
                        if (StrokeResult != null)
                            inkCanvas.Strokes.Remove(StrokeResult);

                        DrawingAttributes drawingAttributes = new DrawingAttributes
                        {
                            Color = Brushes.Red.Color,
                            Width = 1,
                            StylusTip = StylusTip.Ellipse,
                            IgnorePressure = true,
                            FitToCurve = true
                        };

                        StrokeResult = new RectangleStroke(s, drawingAttributes);
                        inkCanvas.Strokes.Add(StrokeResult);
                    }
                    );
                }
            }
        }

        public override void OnMouseRightClick(System.Windows.Controls.InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e) { }

        void GetRectangle(StylusPointCollection pts, Action<StylusPointCollection> exec)
        {
            pts.Add(new StylusPoint(topLeft.X, topLeft.Y));
            pts.Add(new StylusPoint(bottomRight.X, bottomRight.Y));

            exec(pts);
        }
    }
}
