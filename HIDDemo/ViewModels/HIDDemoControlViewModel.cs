using HIDDemo.Models;
using HIDLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UtilityUILib;

namespace HIDDemo.ViewModels
{
    public class HIDDemoControlViewModel : INotifyPropertyChanged
    {
        private IHIDDemoControlModel hidGUIModel;

        #region INotifyPropertyChanged Interface
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            { PropertyChanged(sender, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion

        private ObservableCollection<IMenuItem> hidOPButtonCollection;

        public ObservableCollection<IMenuItem> HIDOPButtonCollection
        {
            get
            {
                return hidOPButtonCollection;
            }

            set
            {
                hidOPButtonCollection = value;
                onPropertyChanged(this, "HIDOPButtonCollection");
            }
        }

        private ObservableCollection<HIDDisplayInfoItem> hidDisplayCollections;

        public ObservableCollection<HIDDisplayInfoItem> HIDDisplayCollections
        {
            get
            {
                return hidDisplayCollections;
            }

            set
            {
                hidDisplayCollections = value;
                onPropertyChanged(this, "HIDDisplayCollections");
            }
        }

        private string messageText;
        public string MessageText
        {
            get
            {
                return messageText;
            }
            set
            {
                messageText = value;
                onPropertyChanged(this, "MessageText");
            }
        }

        public HIDDemoControlViewModel(IHIDDemoControlModel _model)
        {
            hidGUIModel = _model;
            hidOPButtonCollection = hidGUIModel.GetHIDOPButtons;
            hidDisplayCollections = GetHIDDisplayItems;
        }
        List<HIDInfo> hidInfoLst;
        public ObservableCollection<HIDDisplayInfoItem> GetHIDDisplayItems
        {
            get
            {
                hidInfoLst = hidGUIModel.GetHIDInfoCollections;
                ObservableCollection<HIDDisplayInfoItem> revLst = new ObservableCollection<HIDDisplayInfoItem>();
                HIDDisplayInfoItem dInfoItem;
                HIDDisplayItem dItem;
                int fieldIdx = -1;
                foreach (HIDInfo hidInfo in hidInfoLst)
                {
                    dInfoItem = new HIDDisplayInfoItem();
                    dItem = new HIDDisplayItem()
                    {
                        DisplayType = HIDDisplayItemEnum.Lebel,
                        MenuName = "PID",
                    };
                    dInfoItem.HIDDisplayInfoCollections.Add(dItem);
                    dItem = new HIDDisplayItem()
                    {
                        DisplayType = HIDDisplayItemEnum.TextBlock,
                        MenuName = hidInfo.Pid.ToString("X4")
                    };
                    dInfoItem.HIDDisplayInfoCollections.Add(dItem);
                    dItem = new HIDDisplayItem()
                    {
                        DisplayType = HIDDisplayItemEnum.Lebel,
                        MenuName = "VID",
                    };
                    dInfoItem.HIDDisplayInfoCollections.Add(dItem);
                    dItem = new HIDDisplayItem()
                    {
                        DisplayType = HIDDisplayItemEnum.TextBlock,
                        MenuName = hidInfo.Vid.ToString("X4")
                    };
                    dInfoItem.HIDDisplayInfoCollections.Add(dItem);
                    dItem = new HIDDisplayItem()
                    {
                        DisplayType = HIDDisplayItemEnum.Lebel,
                        MenuName = "ManuFactor",
                    };
                    dInfoItem.HIDDisplayInfoCollections.Add(dItem);
                    dItem = new HIDDisplayItem()
                    {
                        DisplayType = HIDDisplayItemEnum.TextBlock,
                        MenuName = hidInfo.Manufacturer
                    };
                    dInfoItem.HIDDisplayInfoCollections.Add(dItem);
                    dItem = new HIDDisplayItem()
                    {
                        DisplayType = HIDDisplayItemEnum.Lebel,
                        MenuName = "CompareStr",
                    };
                    dInfoItem.HIDDisplayInfoCollections.Add(dItem);
                    dItem = new HIDDisplayItem()
                    {
                        DisplayType = HIDDisplayItemEnum.TextBlock,
                        MenuName = hidInfo.HIDCompareStr
                    };
                    dInfoItem.HIDDisplayInfoCollections.Add(dItem);
                    dInfoItem.OnRadioButtonChecked += DInfoItem_OnRadioButtonChecked1;
                    dInfoItem.FieldIdx = ++fieldIdx;
                    revLst.Add(dInfoItem);
                    hidInfo.HIDClose();
                }

                return revLst;
            }
        }

        private void DInfoItem_OnRadioButtonChecked1(HIDDisplayInfoItem infoItem)
        {
            if (infoItem.MenuChecked)
            {
                MessageText = $"InfoItem [{infoItem.FieldIdx}] selected";
            }
        }
    }
}
