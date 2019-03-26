namespace CommonUILib.Models
{
    public class DebugViewItem : ViewItem
    {
        private string _menuName;
        public override string MenuName
        {
            get
            {
                return _menuName;
            }
            set
            {
                SetProperty(ref _menuName, value);
            }
        }
    }
}
