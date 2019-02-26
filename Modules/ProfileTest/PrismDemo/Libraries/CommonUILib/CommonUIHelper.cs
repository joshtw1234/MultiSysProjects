using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUILib
{
    public class CommonUIHelper
    {
        /// <summary>
        /// The _instance.
        /// </summary>
        private static CommonUIHelper _instance;

        /// <summary>
        /// The _region manager.
        /// </summary>
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// The _navigate to command.
        /// </summary>
        private DelegateCommand<string> _navigateToCommand;

        /// <summary>
        /// The _navigate to full screen command.
        /// </summary>
        private DelegateCommand<string> _navigateToFullScreenCommand;

        /// <summary>
        /// The _expand menu command.
        /// </summary>
        private DelegateCommand<string> _expandMenuCommand;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static CommonUIHelper Instance
        {
            get
            {
                return _instance ?? (_instance = new CommonUIHelper());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonUIHelpers"/> class.
        /// </summary>
        private CommonUIHelper()
        {
            this._regionManager = ServiceLocator.Current.GetInstance<RegionManager>();
        }

        /// <summary>
        /// DelegateCommand which executes navigation to the specified region and control.
        /// </summary>
        public DelegateCommand<string> NavigateToCommand => _navigateToCommand ?? (_navigateToCommand = new DelegateCommand<string>(ExecuteNavigateToCommand));

        private void ExecuteNavigateToCommand(string obj)
        {
            if (string.IsNullOrEmpty(obj))
            {
                return;
            }
#if true
            this._regionManager.RequestNavigate("FeatureRegion", new Uri(obj, UriKind.Relative));

#else
            if (Uri.IsWellFormedUriString(data, UriKind.Absolute))
            {
                Process.Start(data);
            }
            else
            {
                this._regionManager.RequestNavigate("FeatureRegion", new Uri(obj, UriKind.Relative));
            }
#endif
        }
    }
}
