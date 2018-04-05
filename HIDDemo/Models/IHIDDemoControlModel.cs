using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityUILib;

namespace HIDDemo.Models
{
    interface IHIDDemoControlModel
    {
        ObservableCollection<IMenuItem> GetHIDOPButtons { get; }
    }
}
