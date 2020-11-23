using Prism.Regions;
using System;
using System.Windows;
using WPFTestPrism7.AppGUIModules.Models.Interfaces;

namespace WPFTestPrism7.AppGUIModules.ViewModels.Features
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
            //MessageBox.Show($"isPass {isPass}");
            continuationCallback(true);
            //isPass = !isPass;
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
