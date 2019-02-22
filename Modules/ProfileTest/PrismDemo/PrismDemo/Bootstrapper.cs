using System.Windows;
using Prism.Unity;
using PrismDemo.Interfaces;
using PrismDemo.Models;

namespace PrismDemo
{
    class Bootstrapper : UnityBootstrapper
    {
        #region Base Bootstrapper.
        protected override DependencyObject CreateShell()
        {
            return base.CreateShell();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            this.RegisterTypeIfMissing(typeof(IMainWindowModel), typeof(MainWindowModel), true);
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
        }
        #endregion
    }
}
