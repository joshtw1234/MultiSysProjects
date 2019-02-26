using HIDDemoModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIDDemoModule.ViewModels
{
    class HIDDemoControlViewModel
    {
        IHIDDemoControlModel _model;
        public HIDDemoControlViewModel(IHIDDemoControlModel model)
        {
            _model = model;
        }
    }
}
