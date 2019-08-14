using CentralModule.Views;
using CommonUILib.Structures;
using Prism.Modularity;
using Prism.Regions;

namespace CentralModule.PrismModules
{
    public class SysHookClientModule : BasePrismModule, IModule
    {
        public SysHookClientModule(IRegionManager regionManager) : base(regionManager)
        {

        }
        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("FeatureRegion", typeof(SysHookClientControl));
        }
    }
}
