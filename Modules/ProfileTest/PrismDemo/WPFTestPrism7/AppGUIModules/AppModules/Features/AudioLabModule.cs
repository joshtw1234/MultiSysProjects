using Prism.Ioc;
using Prism.Regions;
using System;

namespace WPFTestPrism7.AppGUIModules.AppModules.Features
{
    class AudioLabModule : BaseModule
    {
        public AudioLabModule(IRegionManager regionManager) : base(regionManager)
        {

        }
        public override void OnInitialized(IContainerProvider containerProvider)
        {
            //Register View
            _regionManager.RegisterViewWithRegion("FeatureRegion", typeof(Views.Features.AudioLabModuleControl));
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
