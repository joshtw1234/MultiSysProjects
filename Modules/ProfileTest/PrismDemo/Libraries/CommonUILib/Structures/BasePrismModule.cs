using Prism.Regions;

namespace CommonUILib.Structures
{
    public class BasePrismModule
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
        public BasePrismModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

    }
}
