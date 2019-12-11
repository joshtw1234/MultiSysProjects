using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace WPFTestPrism7.AppGUIModules.AppModules
{
    class MenuModule : IModule
    {
        /// <summary>
        /// The _region manager.
        /// </summary>
        protected readonly IRegionManager _regionManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuModule"/> class.
        /// </summary>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        public MenuModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //Register View
            _regionManager.RegisterViewWithRegion("MenuRegion", typeof(Views.MenuModuleControl));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Register Model first, not singleton
            containerRegistry.Register(typeof(Models.Interfaces.IMenuModuleControlModel), typeof(Models.MenuModuleControlModel));
        }
    }
}
