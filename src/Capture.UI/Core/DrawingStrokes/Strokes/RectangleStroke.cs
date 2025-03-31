using System;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Pen = System.Windows.Media.Pen;
using Brush = System.Windows.Media.Brush;
using Point = System.Windows.Point;

namespace Capture.UI.Core.DrawingStrokes.Strokes
{
    internal class RectangleStroke : StrokeBase
    {
        public RectangleStroke(StylusPointCollection pts, DrawingAttributes da)
            : base(pts, da)
        {
            this.StylusPoints = pts;
            this.DrawingAttributes = da;
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            if (drawingContext == null)
            {
                throw new ArgumentNullException("drawingContext");
            }
            if (null == drawingAttributes)
            {
                throw new ArgumentNullException("drawingAttributes");
            }
            Pen pen = new Pen
            {
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round,
                Brush = new SolidColorBrush(drawingAttributes.Color),
                Thickness = drawingAttributes.Width,
                DashStyle = DashStyles.Dash
            };

            Brush BackGround = (Brush)new BrushConverter().ConvertFrom("#1Eff0000");
            drawingContext.DrawRectangle(
                BackGround,
                pen,
                new Rect(new Point(StylusPoints[0].X, StylusPoints[0].Y),
                    new Point(StylusPoints[1].X, StylusPoints[1].Y)));
        }
    }
}
