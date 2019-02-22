using System.Windows;
using Prism.Unity;
using PrismDemo.Interfaces;
using PrismDemo.Models;

namespace PrismDemo
{
    class Bootstrapper : UnityBootstrapper
    {
        #region Base Bootstrapper.
#if true
 
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            this.RegisterTypeIfMissing(typeof(IMainWindowModel), typeof(MainWindowModel), true);
        }

#endif
        #endregion
    }
}
