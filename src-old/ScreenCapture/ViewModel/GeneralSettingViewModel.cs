using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ScreenCapture.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.IO;

namespace ScreenCapture.ViewModel
{
    public class GeneralSettingViewModel : ViewModelBase
    {
        #region UI Variable
        private string _DefaultSavePath;
        public string DefaultSavePath
        {
            get => _DefaultSavePath;
            set => Set(ref _DefaultSavePath, value);
        }
        #endregion

        #region Command
        public RelayCommand<object> ButtonClickCommand { get; private set; }

        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<object>(OnButtonClick);
        }

        #region CommandAction
        /// <summary>
        /// 버튼 클릭 이벤트
        /// </summary>
        /// <param name="param"></param>
        private void OnButtonClick(object param)
        {
            
        }
        #endregion
        #endregion

        #region Field

        #endregion

        public GeneralSettingViewModel()
        {
            InitRelayCommand();
            DefaultSavePath = Path.Combine(DefaultData.BasePath,"Capture");
        }
    }
}
