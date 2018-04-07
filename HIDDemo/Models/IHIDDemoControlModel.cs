using System.Collections.Generic;
using System.Collections.ObjectModel;
using HIDLib;
using UtilityUILib;

namespace HIDDemo.Models
{
    public interface IHIDDemoControlModel
    {
        ObservableCollection<IMenuItem> GetHIDOPButtons { get; }
        List<HIDInfo> GetHIDInfoCollections { get; }
    }
}
