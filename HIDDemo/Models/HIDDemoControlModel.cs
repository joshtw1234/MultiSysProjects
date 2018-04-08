using System.Collections.Generic;
using System.Collections.ObjectModel;
using HIDLib;
using UtilityUILib;

namespace HIDDemo.Models
{
    class HIDDemoControlModel : IHIDDemoControlModel
    {
        List<HIDInfo> lstHIDDevs;

        public List<HIDInfo> GetHIDInfoCollections
        {
            get
            {
                BaseHID bhid = new BaseHID();
                lstHIDDevs = new List<HIDInfo>();
                lstHIDDevs.AddRange(bhid.BrowseHID());
                return lstHIDDevs;
            }
        }

        public void SetHIDClose(int selectHIDIdx)
        {
            lstHIDDevs[selectHIDIdx].HIDClose();
        }

        public void SetHIDOpen(int selectHIDIdx)
        {
            lstHIDDevs[selectHIDIdx].HIDOpen();
        }

        public void SetHIDSend(int selectHIDIdx, byte[] data)
        {

            lstHIDDevs[selectHIDIdx].HIDWrite(data);
            lstHIDDevs[selectHIDIdx].HIDRead(data);
        }
    }
}
