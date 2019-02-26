using CommonUILib.Interfaces;

namespace CommonUILib.Structures
{
    public class TipViewItem
    {
        public IViewItem TipButton { get; set; }
        public IViewItem TipInfo { get; set; }
        public bool TipViewItemVisibility { get; set; }
    }
}
