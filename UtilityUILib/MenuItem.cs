using System.Windows;

namespace UtilityUILib
{
    public class MenuItem : IMenuItem
    {
        public string MenuAutomationId { get; set; }
        public string MenuName { get; set; }
        public string MenuImage { get; set; }
        public string MenuImagePressed { get; set; }
        public string MenuData { get; set; }
        public Style MenuStyle { get; set; }
        public bool MenuEnabled { get; set; }
        public bool MenuChecked { get; set; }
        public bool MenuVisibility { get; set; }
    }
}
