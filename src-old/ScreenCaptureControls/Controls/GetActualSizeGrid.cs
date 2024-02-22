using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ScreenCaptureControls.Controls
{
    [TemplatePart(Name = PART_Grid, Type = typeof(Grid))]
    public class GetActualSizeGrid : Control
    {
        const string PART_Grid = "PART_Grid";

        #region Dependency Property
        public static new readonly DependencyProperty ActualWidthProperty
            = DependencyProperty.Register(nameof(ActualWidth),
                                          typeof(int),
                                          typeof(GetActualSizeGrid),
                                          new PropertyMetadata(0, null));

        public static new readonly DependencyProperty ActualHeightProperty
            = DependencyProperty.Register(nameof(ActualHeight),
                                          typeof(int),
                                          typeof(GetActualSizeGrid),
                                          new PropertyMetadata(0, null));
        #endregion

        #region Fields
        protected Grid grid = null;
        #endregion

        #region Properties 
        public new int ActualWidth
        {
            get => (int)GetValue(ActualWidthProperty);
            set => SetValue(ActualWidthProperty, value);
        }
        
        public new int ActualHeight
        {
            get => (int)GetValue(ActualHeightProperty);
            set => SetValue(ActualHeightProperty, value);
        }
        #endregion

        #region Public Mathod
        public override void OnApplyTemplate()
        {
            
            base.OnApplyTemplate();
            grid = Template.FindName(PART_Grid, this) as Grid;
            if (grid != null)
            {
                // binding event
                grid.SizeChanged += Grid_SizeChanged;
            }
        }
        #endregion

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ActualWidth = (int)grid.ActualWidth;
            ActualHeight = (int)grid.ActualHeight;
        }
    }
}
