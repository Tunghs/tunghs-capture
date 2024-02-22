using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;

namespace ScreenCapture.Model
{
    public class EachClassSettingItem : ObservableObject
    {
        private string _Header;
        public string Header
        {
            get => _Header;
            set => Set(ref _Header, value);
        }

        private int _PositionX;
        public int PositionX
        {
            get => _PositionX;
            set => Set(ref _PositionX, value);
        }

        private int _PositionY;
        public int PositionY
        {
            get => _PositionY;
            set => Set(ref _PositionY, value);
        }

        private int _Width;
        public int Width
        {
            get => _Width;
            set => Set(ref _Width, value);
        }

        private int _Height;
        public int Height
        {
            get => _Height;
            set => Set(ref _Height, value);
        }

        private string _SavePath;
        public string SavePath
        {
            get => _SavePath;
            set => Set(ref _SavePath, value);
        }

        private List<Key> _ShortcutKeyList = new List<Key>();
        public List<Key> ShortcutKeyList
        {
            get => _ShortcutKeyList;
            set => Set(ref _ShortcutKeyList, value);
        }
    }
}
