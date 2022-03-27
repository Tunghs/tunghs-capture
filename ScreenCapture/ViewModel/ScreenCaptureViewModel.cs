using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ScreenCapture.Util;
using ScreenCapture.Model;
using System.Windows.Threading;
using ScreenCaptureCore.Util;

namespace ScreenCapture.ViewModel
{
    public class ScreenCaptureViewModel : ViewModelBase
    {
        #region UI Variable

        private int windowWidth = 508;
        public int WindowWidth
        {
            get => windowWidth; 
            set => Set(ref windowWidth, value);
        }

        private int windowHeight = 606;
        public int WindowHeight
        {
            get => windowHeight;
            set => Set(ref windowHeight, value);
        }

        private int windowLeft;
        public int WindowLeft
        {
            get => windowLeft;
            set => Set(ref windowLeft, value);
        }
        
        private int windowTop;
        public int WindowTop
        {
            get => windowTop;
            set => Set(ref windowTop, value);
        }

        private bool isSettingOpen = false;
        public bool IsSettingOpen
        {
            get => isSettingOpen;
            set => Set(ref isSettingOpen, value);
        }

        private int captureWidth = 500;
        public int CaptureWidth
        {
            get => captureWidth;
            set => Set(ref captureWidth, value);
        }

        private int captureHeight = 500;
        public int CaptureHeight
        {
            get => captureHeight;
            set => Set(ref captureHeight, value);
        }

        private List<string> settings = new List<string>();
        public List<string> Settings
        {
            get => settings;
            set => Set(ref settings, value);
        }

        private string selectedSetting;
        public string SelectedSetting
        {
            get => selectedSetting;
            set => Set(ref selectedSetting, value);
        }

        private bool isEnableSettingBtn = true;
        public bool IsEnableSettingBtn
        {
            get => isEnableSettingBtn;
            set => Set(ref isEnableSettingBtn, value);
        }

        #endregion

        #region Field

        private bool isSaveClipboard = false;
        private KeyboardListener keyboardListener;
        private Capture capture;

        #endregion

        #region ViewModels

        public OptionViewModel OptionViewModel { get; set; }

        #endregion

        #region Contructors

        public ScreenCaptureViewModel()
        {
            InitRelayCommand();
            Initialize();
        }

        private void Initialize()
        {
            DefaultData.Seed();
            capture = new Capture();

            OptionViewModel = new OptionViewModel();
            OptionViewModel.CaptureSettingViewModel._SettingAddEvent += new CaptureSettingViewModel.SettingAddHandler(SendScreenInfo);
            OptionViewModel.CaptureSettingViewModel._SettingChangeEvent += new CaptureSettingViewModel.SettingChangeHandler(ApplySetting);

            keyboardListener = new KeyboardListener();
            keyboardListener.KeyboardDown += new KeyboardDownHandler(KeyboardDown);
        }

        #endregion

        #region Method

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

            if (isSaveClipboard)
            {
                capture.SaveClipboard(captureX, captureY, CaptureWidth, CaptureHeight);
            }
            else
            {
                string savePath = OptionViewModel.GeneralSettingViewModel.DefaultSavePath;
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                capture.SaveFile(captureX, captureY, CaptureWidth, CaptureHeight, savePath);
            }
        }

        /// <summary>
        /// 세팅값과 단축키 매칭해서 해당 세팅값 캡쳐
        /// </summary>
        public void MatchCaptureSetting()
        {
            foreach (var classItem in OptionViewModel.CaptureSettingViewModel.SGClassCollection)
            {
                List<Key> keyList = classItem.ShortcutKeyList;
                if (keyList.All(x => Keyboard.IsKeyDown(x)))
                {
                    if (isSaveClipboard)
                    {
                        capture.SaveClipboard(classItem.PositionX, classItem.PositionY, classItem.Width, classItem.Height);
                    }
                    else
                    {
                        capture.SaveFile(classItem.PositionX, classItem.PositionY, classItem.Width, classItem.Height, classItem.SavePath);
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

        #endregion

        #region CommandAction

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
                    CaptureScreen();
                    break;
            }
        }

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
            isSaveClipboard = (isSaveClipboard) ? false : true;
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
        #endregion
    }
}
