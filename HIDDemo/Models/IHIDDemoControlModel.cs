using System.Collections.Generic;
using System.Collections.ObjectModel;
using HIDLib;
using UtilityUILib;

namespace HIDDemo.Models
{
    public interface IHIDDemoControlModel
    {
        List<HIDInfo> GetHIDInfoCollections { get; }

        void SetHIDOpen(int selectHIDIdx);
        void SetHIDClose(int selectHIDIdx);
        void SetHIDSend(int selectHIDIdx, byte[] data);
    }
}
