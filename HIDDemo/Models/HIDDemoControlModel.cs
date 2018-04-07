using System.Collections.Generic;
using System.Collections.ObjectModel;
using HIDLib;
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

        public List<HIDInfo> GetHIDInfoCollections
        {
            get
            {
                BaseHID bhid = new BaseHID();
                List<HIDInfo> revLst = new List<HIDInfo>();
                revLst.AddRange(bhid.BrowseHID());
                return revLst;
            }
        }
    }
}
