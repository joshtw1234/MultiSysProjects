namespace CommonUILib.Models
{
    public sealed class ProgressBarViewItem : ViewItem
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

        private string _menuMaxValue;
        public string MenuMaxValue
        {
            get
            {
                return _menuMaxValue;
            }
            set
            {
                SetProperty(ref _menuMaxValue, value);
            }
        }
        private string _menuMinValue;
        public string MenuMinValue
        {
            get
            {
                return _menuMinValue;
            }
            set
            {
                SetProperty(ref _menuMinValue, value);
            }
        }
    }
}
