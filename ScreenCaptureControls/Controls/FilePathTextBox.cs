using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenCaptureControls.Controls
{
    [TemplatePart(Name = PART_TextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_Button, Type = typeof(Button))]
    public class FilePathTextBox : Control
    {
        const string PART_TextBox = "PART_TextBox";
        const string PART_Button = "PART_Button";

        #region Dependency Property
        /// <summary>
        /// 파일 저장 경로
        /// </summary>
        public static readonly DependencyProperty FilePathProperty
            = DependencyProperty.Register(nameof(FilePath),
                                          typeof(string),
                                          typeof(FilePathTextBox),
                                          new PropertyMetadata(null, null));

        /// <summary>
        /// 초기 경로
        /// </summary>
        public static readonly DependencyProperty InitPathProperty
            = DependencyProperty.Register(nameof(InitPath),
                                          typeof(string),
                                          typeof(FilePathTextBox),
                                          new PropertyMetadata(null, null));

        /// <summary>
        /// Delete button Command
        /// </summary>
        public static readonly DependencyProperty CommandProperty
            = DependencyProperty.Register(nameof(Command),
                                          typeof(ICommand),
                                          typeof(FilePathTextBox),
                                          new UIPropertyMetadata(null));
        #endregion

        #region  Fields
        protected TextBox textBox = null;
        protected Button button = null;
        #endregion

        #region  Properties 
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }
        
        public string InitPath
        {
            get { return (string)GetValue(InitPathProperty); }
            set { SetValue(InitPathProperty, value); }
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        #endregion

        #region Public Mathod
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            textBox = Template.FindName(PART_TextBox, this) as TextBox;
            if (textBox != null)
            {
                // binding mouse event
                textBox.PreviewDragOver += TextBox_PreviewDragOver;
                textBox.Drop += TextBox_Drop;
            }
            button = Template.FindName(PART_Button, this) as Button;
            if (button != null)
            {
                // binding mouse event
                button.Click += Button_Click;
            }
        }
        #endregion

        #region Private Mathod
        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Handled = true;
            }
        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (IsDirectory(files[0]))
                {
                    FilePath = files[0];
                }
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = InitPath;
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog(Window.GetWindow(this)) == CommonFileDialogResult.Ok)
                {
                    InitPath = dialog.FileName;
                    FilePath = dialog.FileName;
                }
            }
        }

        public static bool IsDirectory(string path)
        {
            if (path == null)
            {
                return false;
            }

            FileAttributes attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
