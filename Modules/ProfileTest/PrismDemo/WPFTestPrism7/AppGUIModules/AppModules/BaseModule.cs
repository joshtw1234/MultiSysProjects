using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace WPFTestPrism7.AppGUIModules.AppModules
{
    abstract class BaseModule : IModule
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
        public BaseModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public abstract void OnInitialized(IContainerProvider containerProvider);

        public abstract void RegisterTypes(IContainerRegistry containerRegistry);
    }
}
