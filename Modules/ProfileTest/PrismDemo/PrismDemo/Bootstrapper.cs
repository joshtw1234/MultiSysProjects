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
using AudioDemoModule.Interfaces;
using PokeGameModule.Interfaces;
using PokeGameModule.Models;
using BigLottoryModule.Interface;
using BigLottoryModule.Models;
using System.Diagnostics;
using System;
using PrismDemo.ViewModels;

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
            this.RegisterTypeIfMissing(typeof(IPokeGameModel), typeof(PokeGameModel), true);
            this.RegisterTypeIfMissing(typeof(IBigLottoryControlModel), typeof(BigLottoryControlModel), true);
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
                moduleCatalog.AddModule(typeof(PokeGameModule.PokeGameModule));
                moduleCatalog.AddModule(typeof(BigLottoryModule.BigLottoryModule));
                //moduleCatalog.AddModule(typeof(AudioDemoModule.AudioDemoModule), InitializationMode.OnDemand);
                //moduleCatalog.AddModule(typeof(HIDDemoModule.HIDDemoModule), InitializationMode.OnDemand);
                //moduleCatalog.AddModule(typeof(PokeGameModule.PokeGameModule), InitializationMode.OnDemand);
                //moduleCatalog.AddModule(typeof(BigLottoryModule.BigLottoryModule), InitializationMode.OnDemand);
            }
        }

        protected override void InitializeModules()
        {
            //base.InitializeModules();
            //System.Threading.Thread.Sleep(3000);
            //var mainVM = Application.Current.MainWindow.DataContext as MainWindowViewModel;
            //mainVM.TextProgress.MenuVisibility = false;
        }
        #endregion
    }
}
