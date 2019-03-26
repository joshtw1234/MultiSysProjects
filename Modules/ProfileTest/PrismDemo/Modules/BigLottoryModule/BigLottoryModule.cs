using BigLottoryModule.Views;
using Prism.Modularity;
using Prism.Regions;

namespace BigLottoryModule
{
    public class BigLottoryModule : IModule
    {
        /// <summary>
        /// The _region manager.
        /// </summary>
        private readonly IRegionManager _regionManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuModule"/> class.
        /// </summary>
        /// <param name="regionManager">
        /// The region manager.
        /// </param>
        public BigLottoryModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("FeatureRegion", typeof(BigLottoryControl));
        }
    }
}
