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

namespace ScreenCapture.ViewModel
{
    public class OptionViewModel : ViewModelBase
    {
        #region UI Variable

        #endregion

        #region Command
        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }

        private void InitRelayCommand()
        {
            ClosingCommand = new RelayCommand<CancelEventArgs>(Closing);
        }

        #region CommandAction
        /// <summary>
        /// 창 종료시 변경 내역 업데이트 이벤트 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Closing(CancelEventArgs e)
        {
            CaptureSettingViewModel.SetSettingChange();
        }
        #endregion
        #endregion
        // 추가할예정
        #region Field

        #endregion

        public CaptureSettingViewModel CaptureSettingViewModel { get; private set; }
        public GeneralSettingViewModel GeneralSettingViewModel { get; private set; }

        public OptionViewModel()
        {
            InitRelayCommand();
            CaptureSettingViewModel = new CaptureSettingViewModel();
            GeneralSettingViewModel = new GeneralSettingViewModel();
        }
    }
}
