using System.Windows;
using AudioDemoModule.Models;
using MenuModule.Interfaces;
using MenuModule.Models;
using Prism.Modularity;
using Prism.Unity;
using PrismDemo.Interfaces;
using PrismDemo.Models;

namespace PrismDemo
{
    class Bootstrapper : UnityBootstrapper
    {
        #region Base Bootstrapper.
        
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            //Register the UI model here.
            this.RegisterTypeIfMissing(typeof(IMainWindowModel), typeof(MainWindowModel), true);
            this.RegisterTypeIfMissing(typeof(IMenuControlModel), typeof(MenuControlModel), true);
            this.RegisterTypeIfMissing(typeof(IAudioDemoControlModel), typeof(AudioDemoControlModel), true);
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            var moduleCatalog = this.ModuleCatalog as ModuleCatalog;

            // Register the UI modules here.
            if (moduleCatalog != null)
            {
                moduleCatalog.AddModule(typeof(MenuModule.MenuModule));
                moduleCatalog.AddModule(typeof(AudioDemoModule.AudioDemoModule));
            }
        }
        #endregion
    }
}
