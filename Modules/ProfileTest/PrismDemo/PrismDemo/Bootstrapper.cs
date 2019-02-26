using System.Windows;
using AudioDemoModule.Models;
using HIDDemoModule.Interfaces;
using HIDDemoModule.Models;
using MenuModule.Interfaces;
using MenuModule.Models;
using PrismDemo.Interfaces;
using PrismDemo.Models;
using PrismDemo.Views;
using Prism.Events;
using Prism.Modularity;
using Prism.Unity;
//Add This Line for using unity extension.
using Microsoft.Practices.Unity;


namespace PrismDemo
{
    class Bootstrapper : UnityBootstrapper
    {
        #region Base Bootstrapper.

        protected override DependencyObject CreateShell()
        {
            IEventAggregator eventAggregator = new EventAggregator();
            Container.RegisterInstance<IEventAggregator>(eventAggregator);

            return Container.Resolve<MainWindow>();

        }

        /// <summary>
        /// The initialize shell.
        /// </summary>
        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            //Register the UI model here.
            this.RegisterTypeIfMissing(typeof(IMainWindowModel), typeof(MainWindowModel), true);
            this.RegisterTypeIfMissing(typeof(IMenuControlModel), typeof(MenuControlModel), true);
            this.RegisterTypeIfMissing(typeof(IAudioDemoControlModel), typeof(AudioDemoControlModel), true);
            this.RegisterTypeIfMissing(typeof(IHIDDemoControlModel), typeof(HIDDemoControlModel), true);
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
                moduleCatalog.AddModule(typeof(HIDDemoModule.HIDDemoModule));
            }
        }
        #endregion
    }
}
