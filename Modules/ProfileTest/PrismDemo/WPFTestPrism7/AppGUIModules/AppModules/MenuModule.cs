using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace WPFTestPrism7.AppGUIModules.AppModules
{
    class MenuModule : BaseModule
    {
        public MenuModule(IRegionManager regionManager) : base(regionManager)
        {
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            //Register View
            _regionManager.RegisterViewWithRegion("MenuRegion", typeof(Views.MenuModuleControl));
            _regionManager.RegisterViewWithRegion("FeatureRegion", typeof(Views.Features.HIDDemoControl));
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Register Model first, not singleton
            containerRegistry.Register(typeof(Models.Interfaces.IMenuModuleControlModel), typeof(Models.MenuModuleControlModel));
            containerRegistry.Register(typeof(Models.Interfaces.IHIDDemoControlModel), typeof(Models.HIDDemoControlModel));
        }
    }
}
