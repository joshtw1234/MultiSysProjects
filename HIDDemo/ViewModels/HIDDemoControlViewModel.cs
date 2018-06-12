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

        List<HIDInfo> hidInfoLst;

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

        private MessageTextDCT messageText;
        public MessageTextDCT MessageText
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
            messageText = hidGUIModel.GetMessageText;
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
                    },
                    new HIDOPButtonDT
                    {
                        MenuName = HIDDemoControlConstants.HeadSetCMD,
                        MenuCommand = new MyCommond<string>(OnBtnClick)
                    }
                };
            }
        }

        private void OnBtnClick(string obj)
        {
            bool btnCloseStatus = true;
            bool isAsync = false;
            if (hidInfoLst[selectHIDIdx].InfoStruct.Pid == 0x8824)
            {
                isAsync = true;
            }
            if (obj.Equals(HIDDemoControlConstants.OpenHID))
            {
                if (!hidGUIModel.SetHIDOpen(selectHIDIdx, isAsync))
                {
                    btnCloseStatus = false;
                }
            }
            if (obj.Equals(HIDDemoControlConstants.CloseHID))
            {
                hidGUIModel.SetHIDClose(selectHIDIdx);
                btnCloseStatus = false;
            }
            if (obj.Equals(HIDDemoControlConstants.SendHID))
            {
                byte[] data = new byte[64];
                data[0] = 0x80;
                data[1] = 0x01;
                hidGUIModel.SetHIDSend(selectHIDIdx, data, isAsync);
            }
            if (obj.Equals(HIDDemoControlConstants.HeadSetCMD))
            {
                byte[] data2 = new byte[15];
                //data2[0] = 0xFF;
                data2[0] = 0x05;
                data2[1] = 0x02;
                hidGUIModel.SetHIDSend(selectHIDIdx, data2, isAsync);
            }

            HIDOPButtonDT btnClose = HIDOPButtonCollection.FirstOrDefault(x => x.MenuName.Equals(HIDDemoControlConstants.CloseHID)) as HIDOPButtonDT;
            HIDOPButtonDT btnOpen = HIDOPButtonCollection.FirstOrDefault(x => x.MenuName.Equals(HIDDemoControlConstants.OpenHID)) as HIDOPButtonDT;
            HIDOPButtonDT btnSend = HIDOPButtonCollection.FirstOrDefault(x => x.MenuName.Equals(HIDDemoControlConstants.SendHID)) as HIDOPButtonDT;
            HIDOPButtonDT btnHead = HIDOPButtonCollection.FirstOrDefault(x => x.MenuName.Equals(HIDDemoControlConstants.HeadSetCMD)) as HIDOPButtonDT;
            btnClose.BtnEnabled = btnSend.BtnEnabled = btnHead.BtnEnabled = btnCloseStatus;
            btnOpen.BtnEnabled = !btnCloseStatus;
        }

        private ObservableCollection<HIDDisplayInfoItem> GetHIDDisplayItems
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
                        MenuName = hidInfo.InfoStruct.Pid.ToString("X4")
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
                        MenuName = hidInfo.InfoStruct.Vid.ToString("X4")
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
                        MenuName = hidInfo.InfoStruct.Manufacturer
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
                        MenuName = hidInfo.InfoStruct.HIDCompareStr
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
                MessageText.MsgText += $"\r\nInfoItem [{selectHIDIdx}] selected";
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
