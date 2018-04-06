using System.Collections.ObjectModel;
using UtilityUILib;

namespace HIDDemo.Models
{
    class HIDDemoControlModel : IHIDDemoControlModel
    {
        public ObservableCollection<IMenuItem> GetHIDOPButtons
        {
            get
            {
                return new ObservableCollection<IMenuItem>()
                {
                    new MenuItem
                    {
                        MenuName = "Browse HID"
                    },
                    new MenuItem
                    {
                        MenuName = "Open HID"
                    },
                    new MenuItem
                    {
                        MenuName = "Close HID"
                    }
                };
            }
        }
    }
}
