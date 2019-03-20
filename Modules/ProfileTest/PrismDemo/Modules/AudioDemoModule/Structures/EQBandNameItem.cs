using CommonUILib.Models;

namespace AudioDemoModule.Structures
{
    class EQBandNameItem : ViewItem
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
        private bool _menuVisibility;
        public override bool MenuVisibility
        {
            get => _menuVisibility;
            set
            {
                SetProperty(ref _menuVisibility, value);
            }
        }
    }
}
