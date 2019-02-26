using CommonUILib.Models;

namespace CommonUILib.Structures
{
    public class TipMessageItem : ViewItem
    {
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
