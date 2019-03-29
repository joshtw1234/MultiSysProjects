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
using Microsoft.Practices.ServiceLocation;
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
            }
        }

        protected override void InitializeModules()
        {
            var mainVM = Application.Current.MainWindow.DataContext as MainWindowViewModel;
            var resu = mainVM.StartEntireProgress();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //In here to control module initialize.
            this.Container.Resolve<MenuModule.MenuModule>().Initialize();
            sw.Stop();
            Console.WriteLine($"MenuModule {sw.Elapsed.TotalMilliseconds}");
            sw.Restart();
            this.Container.Resolve<AudioDemoModule.AudioDemoModule>().Initialize();
            sw.Stop();
            Console.WriteLine($"AudioDemoModule {sw.Elapsed.TotalMilliseconds}");
            sw.Restart();
            this.Container.Resolve<HIDDemoModule.HIDDemoModule>().Initialize();
            sw.Stop();
            Console.WriteLine($"HIDDemoModule {sw.Elapsed.TotalMilliseconds}");
            sw.Restart();
            this.Container.Resolve<PokeGameModule.PokeGameModule>().Initialize();
            sw.Stop();
            Console.WriteLine($"PokeGameModule {sw.Elapsed.TotalMilliseconds}");
            sw.Restart();
            this.Container.Resolve<BigLottoryModule.BigLottoryModule>().Initialize();
            sw.Stop();
            Console.WriteLine($"BigLottoryModule {sw.Elapsed.TotalMilliseconds}");
            System.Threading.Thread.Sleep(2000);
            
            mainVM.TextProgress.MenuVisibility = false;
        }
        #endregion
    }
}
