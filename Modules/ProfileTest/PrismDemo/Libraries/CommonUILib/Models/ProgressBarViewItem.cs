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
        //No need update from model
        public string MenuMaxValue { get; set; }
        public string MenuMinValue { get; set; }
    }
}
