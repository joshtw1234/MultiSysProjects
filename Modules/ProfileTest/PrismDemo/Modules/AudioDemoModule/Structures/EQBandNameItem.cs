using CommonUILib.Models;

namespace AudioDemoModule.Structures
{
    class EQBandNameItem : BaseViewItem
    {
        private string _menuName;
        public override string MenuName
        {
            get => _menuName;
            set
            {
                SetProperty(ref _menuName, value);
            }
        }
    }
}
