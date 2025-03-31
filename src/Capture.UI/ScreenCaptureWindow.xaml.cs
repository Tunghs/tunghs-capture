using Capture.UI.Core.DrawingStrokes;
using Capture.UI.Core.DrawingStrokes.Draw;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Capture.UI
{
    /// <summary>
    /// ScreenCaptureWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ScreenCaptureWindow : Window
    {
        private DrawStrokeBase drawingInkCanvsStroke;

        public ScreenCaptureWindow(BitmapImage source)
        {
            InitializeComponent();

            this.Loaded += ScreenCaptureWindow_Loaded;
        }

        private void ScreenCaptureWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PART_Canvas.DefaultDrawingAttributes.Color = System.Windows.Media.Colors.Red;
            PART_Canvas.UpdateLayout();

            PART_Canvas.PreviewMouseLeftButtonDown += InkCanvas_PreviewMouseLeftButtonDown;
            PART_Canvas.MouseMove += InkCanvas_PreviewMouseMove;
            PART_Canvas.MouseLeftButtonUp += InkCanvas_PreviewMouseLeftButtonUp;
            PART_Canvas.SelectionMoved += InkCanvas_SelectionResizedOrMoved;
            PART_Canvas.SelectionResized += InkCanvas_SelectionResizedOrMoved;

            drawingInkCanvsStroke = new DrawRectangle();
        }

        private void InkCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PART_Canvas.GetSelectedStrokes().Count != 0)
            {
                System.Windows.Rect rect = PART_Canvas.GetSelectionBounds();
                rect.Inflate(14, 14);
                if (!rect.Contains(e.GetPosition(PART_Canvas)))
                {
                    PART_Canvas.EditingMode = InkCanvasEditingMode.None;
                }
                else
                {
                    return;
                }
            }

            if (!PART_Canvas.IsMouseCaptured)
                PART_Canvas.CaptureMouse();

            PART_Canvas.Strokes.Clear();
            drawingInkCanvsStroke.OnMouseDown(PART_Canvas, e);
        }

        private void InkCanvas_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (PART_Canvas.GetSelectedStrokes().Count != 0)
                return;

            drawingInkCanvsStroke.OnMouseMove(PART_Canvas, e);
        }

        private void InkCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PART_Canvas.IsMouseCaptured)
                PART_Canvas.ReleaseMouseCapture();

            StrokeCollection strokes = new StrokeCollection();
            if (drawingInkCanvsStroke.StrokeResult == null)
            {
                return;
            }

            strokes = new StrokeCollection() { drawingInkCanvsStroke.StrokeResult };
            PART_Canvas.Select(strokes);

            // AlgoRect Drawing 이벤트 발생
            var item = PART_Canvas.GetSelectedStrokes().LastOrDefault();
            if (item == null)
                return;
        }

        private void InkCanvas_SelectionResizedOrMoved(object sender, EventArgs e)
        {
            var item = PART_Canvas.GetSelectedStrokes().LastOrDefault();
            if (item == null)
                return;
        }
    }
}
