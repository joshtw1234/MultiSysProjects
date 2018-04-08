using HIDDemo.Models;
using HIDLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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


        private int selectHIDIdx = -1;
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
            hidOPButtonCollection = GetHIDOPButtons;
            hidDisplayCollections = GetHIDDisplayItems;
        }
        
        private ObservableCollection<IMenuItem> GetHIDOPButtons
        {
            get
            {
                return new ObservableCollection<IMenuItem>()
                {
                    new HIDOPButtonDT
                    {
                        MenuName = HIDDemoControlConstants.BrowseHID,
                        MenuCommand = new MyCommond<string>(OnBtnClick)
                    },
                    new HIDOPButtonDT
                    {
                        MenuName = HIDDemoControlConstants.OpenHID,
                        MenuCommand = new MyCommond<string>(OnBtnClick)
                    },
                    new HIDOPButtonDT
                    {
                        MenuName = HIDDemoControlConstants.CloseHID,
                        MenuCommand = new MyCommond<string>(OnBtnClick)
                    },
                    new HIDOPButtonDT
                    {
                        MenuName = HIDDemoControlConstants.SendHID,
                        MenuCommand = new MyCommond<string>(OnBtnClick)
                    }
                };
            }
        }

        private void OnBtnClick(string obj)
        {
            bool btnCloseStatus = false;
            if (obj.Equals(HIDDemoControlConstants.OpenHID))
            {
                hidGUIModel.SetHIDOpen(selectHIDIdx);
                btnCloseStatus = true;
            }
            if (obj.Equals(HIDDemoControlConstants.CloseHID))
            {
                hidGUIModel.SetHIDClose(selectHIDIdx);
            }
            if (obj.Equals(HIDDemoControlConstants.SendHID))
            {
                byte[] data = new byte[64];
                hidGUIModel.SetHIDSend(selectHIDIdx, data);
            }
            HIDOPButtonDT btnClose = HIDOPButtonCollection.FirstOrDefault(x => x.MenuName.Equals(HIDDemoControlConstants.CloseHID)) as HIDOPButtonDT;
            HIDOPButtonDT btnOpen = HIDOPButtonCollection.FirstOrDefault(x => x.MenuName.Equals(HIDDemoControlConstants.OpenHID)) as HIDOPButtonDT;
            HIDOPButtonDT btnSend = HIDOPButtonCollection.FirstOrDefault(x => x.MenuName.Equals(HIDDemoControlConstants.SendHID)) as HIDOPButtonDT;
            btnClose.BtnEnabled = btnSend.BtnEnabled = btnCloseStatus;
            btnOpen.BtnEnabled = !btnCloseStatus;
        }

        private ObservableCollection<HIDDisplayInfoItem> GetHIDDisplayItems
        {
            get
            {
                List<HIDInfo> hidInfoLst = hidGUIModel.GetHIDInfoCollections;
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
                if (selectHIDIdx > -1)
                {
                    //Close previous HID
                    hidGUIModel.SetHIDClose(selectHIDIdx);
                }
                HIDOPButtonDT btnClose = HIDOPButtonCollection.FirstOrDefault(x => x.MenuName.Equals(HIDDemoControlConstants.CloseHID)) as HIDOPButtonDT;
                HIDOPButtonDT btnSend = HIDOPButtonCollection.FirstOrDefault(x => x.MenuName.Equals(HIDDemoControlConstants.SendHID)) as HIDOPButtonDT;
                btnClose.BtnEnabled = btnSend.BtnEnabled = false;
                //Select new one.
                selectHIDIdx = infoItem.FieldIdx;
                HIDOPButtonDT btnOpen = HIDOPButtonCollection.FirstOrDefault(x=>x.MenuName.Equals(HIDDemoControlConstants.OpenHID)) as HIDOPButtonDT;
                btnOpen.BtnEnabled = true;
                MessageText = $"InfoItem [{selectHIDIdx}] selected";
            }
        }
    }

    public class HIDOPButtonDT : MenuItem, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Interface
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            { PropertyChanged(sender, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion

        public bool BtnEnabled
        {
            get
            {
                return MenuEnabled;
            }
            set
            {
                MenuEnabled = value;
                onPropertyChanged(this, "BtnEnabled");
            }
        }
    }
}
