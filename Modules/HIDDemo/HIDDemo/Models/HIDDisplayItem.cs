using System.Collections.ObjectModel;
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

    public class HIDDisplayInfoItem : MenuItem
    {
        public delegate void RadioButtonChecked(HIDDisplayInfoItem infoItem);

        public event RadioButtonChecked OnRadioButtonChecked;

        public int FieldIdx = 0;
        public ObservableCollection<IMenuItem> HIDDisplayInfoCollections { get; set; } = new ObservableCollection<IMenuItem>();

        public bool RadioBtnChecked
        {
            get
            {
                return MenuChecked;
            }
            set
            {
                MenuChecked = value;
                OnRadioButtonChecked(this);
            }
        }
    }
}
