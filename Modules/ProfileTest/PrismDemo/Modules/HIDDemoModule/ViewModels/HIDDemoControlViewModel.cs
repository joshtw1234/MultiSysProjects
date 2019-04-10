using HIDDemoModule.Interfaces;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HIDDemoModule.ViewModels
{
    class HIDDemoControlViewModel : IConfirmNavigationRequest
    {
        IHIDDemoControlModel _model;
        public HIDDemoControlViewModel(IHIDDemoControlModel model)
        {
            _model = model;
        }

        bool isPass = false;
        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            MessageBox.Show($"isPass {isPass}");
            continuationCallback(isPass);
            isPass = !isPass;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
