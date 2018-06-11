using System.Collections.Generic;
using System.ComponentModel;
using HIDLib;
using UtilityUILib;

namespace HIDDemo.Models
{
    public interface IHIDDemoControlModel
    {
        List<HIDInfo> GetHIDInfoCollections { get; }
        MessageTextDCT GetMessageText { get; }

        bool SetHIDOpen(int selectHIDIdx);
        void SetHIDClose(int selectHIDIdx);
        void SetHIDSend(int selectHIDIdx, byte[] data);
    }

    public class MessageTextDCT : MenuItem, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Interface
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            { PropertyChanged(sender, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion

        public string MsgText
        {
            get
            {
                return MenuName;
            }
            set
            {
                MenuName = value;
                onPropertyChanged(this, "MsgText");
            }
        }
    }
}
