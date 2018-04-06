using UtilityUILib;

namespace HIDDemo.Models
{
    public class HIDDisplayItem : MenuItem
    {
        private HIDDisplayItemEnum displayType;
        public HIDDisplayItemEnum DisplayType
        {
            get
            {
                return displayType;
            }
            set
            {
                displayType = value;
            }
        }
    }
}
