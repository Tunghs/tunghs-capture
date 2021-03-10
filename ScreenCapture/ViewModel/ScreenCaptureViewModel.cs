using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ScreenCaptureControls.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ScreenCapture.Util;
using ScreenCapture.Model;
using System.Diagnostics;
using System.Windows.Threading;
using ScreenCaptureCore.Util;

namespace ScreenCapture.ViewModel
{
    public class ScreenCaptureViewModel : ViewModelBase
    {
        #region UI Variable
        private int _WindowWidth = 508;
        public int WindowWidth
        {
            get => _WindowWidth; 
            set => Set(ref _WindowWidth, value);
        }

        private int _WindowHeight = 606;
        public int WindowHeight
        {
            get => _WindowHeight;
            set => Set(ref _WindowHeight, value);
        }

        private int _WindowLeft;
        public int WindowLeft
        {
            get => _WindowLeft;
            set => Set(ref _WindowLeft, value);
        }
        
        private int _WindowTop;
        public int WindowTop
        {
            get => _WindowTop;
            set => Set(ref _WindowTop, value);
        }

        private bool _IsSettingOpen = false;
        public bool IsSettingOpen
        {
            get => _IsSettingOpen;
            set => Set(ref _IsSettingOpen, value);
        }

        private int _CaptureWidth = 500;
        public int CaptureWidth
        {
            get => _CaptureWidth;
            set => Set(ref _CaptureWidth, value);
        }

        private int _CaptureHeight = 500;
        public int CaptureHeight
        {
            get => _CaptureHeight;
            set => Set(ref _CaptureHeight, value);
        }

        private List<string> _Settings = new List<string>();
        public List<string> Settings
        {
            get => _Settings;
            set => Set(ref _Settings, value);
        }

        private string _SelectedSetting;
        public string SelectedSetting
        {
            get => _SelectedSetting;
            set => Set(ref _SelectedSetting, value);
        }

        private bool _IsEnableSettingBtn = true;
        public bool IsEnableSettingBtn
        {
            get => _IsEnableSettingBtn;
            set => Set(ref _IsEnableSettingBtn, value);
        }
        #endregion

        #region Command
        public RelayCommand<object> ButtonClickCommand { get; private set; }
        public RelayCommand<TextCompositionEventArgs> PreviewTextInputCommand { get; private set; }
        public RelayCommand<KeyEventArgs> TextBoxKeyDownCommand { get; private set; }
        public RelayCommand<KeyEventArgs> WindowPreviewKeyDownCommand { get; private set; }
        public RelayCommand<MouseButtonEventArgs> WindowPreviewMouseDoubleClickCommand { get; private set; }
        public RelayCommand<object> ToggledCommand { get; private set; }


        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<object>(OnButtonClick);
            PreviewTextInputCommand = new RelayCommand<TextCompositionEventArgs>(OnPreviewTextInput);
            TextBoxKeyDownCommand = new RelayCommand<KeyEventArgs>(OnTextBoxKeyDown);
            WindowPreviewKeyDownCommand = new RelayCommand<KeyEventArgs>(WindowPreviewKeyDown);
            WindowPreviewMouseDoubleClickCommand = new RelayCommand<MouseButtonEventArgs>(OnWindowPreviewMouseDoubleClick);
            ToggledCommand = new RelayCommand<object>(OnToggled);
        }

        #region CommandAction

        /// <summary>
        /// 윈도우 키 입력 이벤트
        /// </summary>
        /// <param name="e"></param>
        private void WindowPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.C:
                    CaptureScreen();
                    break;
                case Key.O:
                    OpenOptionWindow();
                    break;
                case Key.S:
                    break;
            }

            MatchCaptureSetting();
        }

        /// <summary>
        /// 윈도우 타이틀바 더블클릭했을 때 전체화면 안되게끔 처리함
        /// </summary>
        /// <param name="e"></param>
        private void OnWindowPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void OnTextBoxKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SetSize();
            }
        }

        /// <summary>
        /// Label 토글 스위치
        /// </summary>
        /// <param name="param"></param>
        private void OnToggled(object param)
        {
            _IsSaveClipboard = (_IsSaveClipboard) ? false : true;
        }

        /// <summary>
        /// 버튼 클릭 이벤트
        /// </summary>
        /// <param name="param"></param>
        private void OnButtonClick(object param)
        {
            switch (param.ToString())
            {
                case "SetSize":
                    SetSize();
                    break;
                case "SetSetting":
                    SetSetting();
                    break;
                case "OpenSettingWindow":
                    OpenOptionWindow();
                    break;
                case "Capture":
                    CaptureClick();
                    break;
            }
        }

        /// <summary>
        /// 캡처 사이즈 텍스트 박스에 숫자만 입력 받음
        /// </summary>
        /// <param name="e"></param>
        private void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

        /// <summary>
        /// 세팅창 열기
        /// </summary>
        private void OpenOptionWindow()
        {
            OptionWindow optionWindow = new OptionWindow() { DataContext = OptionViewModel };
            optionWindow.Owner = Application.Current.MainWindow;
            optionWindow.Show();

            IsEnableSettingBtn = false;
        }

        /// <summary>
        /// 윈도우 사이즈 세팅
        /// </summary>
        private void SetSize()
        {
            WindowWidth = CaptureWidth + 8;
            WindowHeight = CaptureHeight + 106;
        }

        private void SetSetting()
        {
            foreach (var SGClass in OptionViewModel.CaptureSettingViewModel.SGClassCollection)
            {
                if (SGClass.Header == SelectedSetting)
                {
                    SGClass.Width = CaptureWidth;
                    SGClass.Height = CaptureHeight;
                    SGClass.PositionX = WindowLeft;
                    SGClass.PositionY = WindowTop;
                }
            }
        }

        private void CaptureClick()
        {
            CaptureScreen();
        }
        #endregion
        #endregion

        #region Field
        private bool _IsSaveClipboard = false;
        #endregion
        
        public OptionViewModel OptionViewModel { get; set; }
        KeyboardListener KeyboardListener = new KeyboardListener();

        public ScreenCaptureViewModel()
        {
            InitRelayCommand();
            DefaultData.Seed();

            OptionViewModel = new OptionViewModel();
            OptionViewModel.CaptureSettingViewModel._SettingAddEvent += new CaptureSettingViewModel.SettingAddHandler(SendScreenInfo);
            OptionViewModel.CaptureSettingViewModel._SettingChangeEvent += new CaptureSettingViewModel.SettingChangeHandler(ApplySetting);
            KeyboardListener.KeyboardDown += new KeyboardDownHandler(KeyboardDown);
        }

        /// <summary>
        /// Global keyboard hooking
        /// </summary>
        public void KeyboardDown()
        {
            MatchCaptureSetting();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CaptureScreen()
        {
            int captureX = WindowLeft + 4;
            int captureY = WindowTop + 77;

            CaptureController captureController = new CaptureController();
            if (_IsSaveClipboard)
            {
                captureController.SaveToClipboard(CaptureWidth, 
                    CaptureHeight, captureX, captureY);
            }
            else
            {
                string savePath = OptionViewModel.GeneralSettingViewModel.DefaultSavePath;
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                captureController.SaveToPath(CaptureWidth, 
                    CaptureHeight, captureX, captureY, savePath);
            }
        }

        /// <summary>
        /// 세팅값과 단축키 매칭해서 해당 세팅값 캡쳐
        /// </summary>
        public void MatchCaptureSetting()
        {
            foreach (var classItem in OptionViewModel.CaptureSettingViewModel.SGClassCollection)
            {
                CaptureController captureController = new CaptureController();
                List<Key> keyList = classItem.ShortcutKeyList;
                if (keyList.All(x => Keyboard.IsKeyDown(x)))
                {
                    if (_IsSaveClipboard)
                    {
                        captureController.SaveToClipboard(classItem.Width, 
                            classItem.Height, classItem.PositionX, classItem.PositionY);
                    }
                    else
                    {
                        captureController.SaveToPath(classItem.Width, 
                            classItem.Height, classItem.PositionX, classItem.PositionY, classItem.SavePath);
                    }    
                }
            }  
        }

        /// <summary>
        /// delay
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public DateTime Delay(int ms)
        {
            DateTime thisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime afterWards = thisMoment.Add(duration);

            while (afterWards >= thisMoment)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                thisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }

        /// <summary>
        /// Setting 창에서 항목 추가시 현재 스크린의 정보를 업데이트해서 전달함
        /// </summary>
        private void SendScreenInfo()
        {
            OptionViewModel.CaptureSettingViewModel.Width = CaptureWidth;
            OptionViewModel.CaptureSettingViewModel.Height = CaptureHeight;
            OptionViewModel.CaptureSettingViewModel.PositionX = WindowLeft + 4;
            OptionViewModel.CaptureSettingViewModel.PositionY = WindowTop + 77;
        }

        /// <summary>
        /// 세팅창 종료 후 설정한 값 적용
        /// </summary>
        private void ApplySetting(List<string> settingList)
        {
            Settings = settingList;
            IsEnableSettingBtn = true;
        }
    }
}
