using CommonUILib.Interfaces;
using CommonUILib.Models;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Modularity;
using PrismDemo.Interfaces;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PrismDemo.ViewModels
{
    class MainWindowViewModel : IProgressBarControlViewModel
    {
        IMainWindowModel _model;
        public IViewItem TextProgress { get; set; }
        public IViewItem ViewProgressBar { get; set; }

        private DelegateCommand<EventArgs> _windowSourceInitializedEvent;

        public DelegateCommand<EventArgs> WindowSourceInitializedEvent => _windowSourceInitializedEvent ?? (_windowSourceInitializedEvent = new DelegateCommand<EventArgs>(OnWindowSourceInitialized));
        private DelegateCommand<RoutedEventArgs> _windowLoadedEvent;

        public DelegateCommand<RoutedEventArgs> WindowLoadedEvent => _windowLoadedEvent ?? (_windowLoadedEvent = new DelegateCommand<RoutedEventArgs>(OnWindowLoaded));

        private void OnWindowLoaded(RoutedEventArgs obj)
        {
            //throw new NotImplementedException();
        }

        private void OnWindowSourceInitialized(EventArgs obj)
        {
            Task.Run(new Action(() =>
            {
                StartLoadModuel();
                //LoadModuleOffLine();
            }));
        }


        public MainWindowViewModel(IMainWindowModel model)
        {
            _model = model;
            TextProgress = new DebugViewItem()
            {
                MenuName = "Please wait processing....",
                MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style,
                MenuVisibility = true
            };

            ViewProgressBar = new ProgressBarViewItem()
            {
                MenuName = "50",
                MenuMinValue = "0",
                MenuMaxValue = "100",
                MenuStyle = Application.Current.Resources["CustomProgressBar"] as Style,
            };
            var resu = StartEntireProgress();
            _model.InitializeSystemHook();
            CommonUILib.PrismDemoPubSubEvent<bool>.Instance.Subscribe(OnModuleLoaded);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ViewProgressBar.MenuName = e.ProgressPercentage.ToString();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i += 10)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(100);
            }
        }

        private void OnModuleLoaded(bool obj)
        {
            TextProgress.MenuVisibility = obj;
        }

        private async Task StartEntireProgress()
        {
            await Task.Factory.StartNew(() =>
            {
                while (TextProgress.MenuVisibility)
                {
                    for (int i = 0; i <= 100; i += 10)
                    {
                        Thread.Sleep(10);
                        ViewProgressBar.MenuName = i.ToString();
                    }
                }
            });
        }

        /// <summary>
        /// Reference 
        /// https://www.infragistics.com/community/blogs/b/blagunas/posts/prism-dynamically-discover-and-load-modules-at-runtime
        /// </summary>
        private void StartLoadModuel()
        {
            var disp = Application.Current.Dispatcher;
            
            //Prism.Modularity.IModuleManager moduleManager = this.Container.Resolve<IModuleManager>();
            //IModuleCatalog cat = this.Container.Resolve<IModuleCatalog>();
            IModuleCatalog moduleCatlog = ServiceLocator.Current.GetInstance<IModuleCatalog>();
            IModuleManager manager = ServiceLocator.Current.GetInstance<IModuleManager>();
            foreach (var mo in moduleCatlog.Modules)
            {
                Thread.Sleep(500);
                if (mo.State == ModuleState.Initialized) continue;
                if (disp.CheckAccess())
                    manager.LoadModule(mo.ModuleName);
                else
                    disp.BeginInvoke((Action)delegate { manager.LoadModule(mo.ModuleName); });
            }

            CommonUILib.PrismDemoPubSubEvent<bool>.Instance.Publish(false);
        }

        private void LoadModuleOffLine()
        {
            IModuleCatalog moduleCatlog = ServiceLocator.Current.GetInstance<IModuleCatalog>();
            IModuleManager manager = ServiceLocator.Current.GetInstance<IModuleManager>();
            string modulePath = AppDomain.CurrentDomain.BaseDirectory;
            string[] strModuleLst = Directory.GetFiles(modulePath, "*.dll", SearchOption.TopDirectoryOnly);
            Assembly plugin;
            Type[] types = null;

            foreach (string strModule in strModuleLst)
            {
#if true
                /*
                 * This is a way for load prism module off line.
                 * Looks like legacy module design, very interesting.
                 */
                Assembly moduleAssembly = AppDomain.CurrentDomain.GetAssemblies().First(asm => asm.FullName == typeof(IModule).Assembly.FullName);
                Type IModuleType = moduleAssembly.GetType(typeof(IModule).FullName);
                //load our newly added assembly
                plugin = Assembly.LoadFile(strModule);
                var moduleInfos = plugin.GetExportedTypes()        
                    .Where(IModuleType.IsAssignableFrom)        
                    .Where(t => t != IModuleType)
                    .Where(t => !t.IsAbstract).Select(t => CreateModuleInfo(t));
                foreach (ModuleInfo moduleInfo in moduleInfos)
                {
                    //add the ModuleInfo to the catalog so it can be loaded
                    moduleCatlog.AddModule(moduleInfo);

                    //now load the module using the Dispatcher because the FileSystemWatcher.Created even occurs on a separate thread
                    //and we need to load our module into the main thread.
                    var d = Application.Current.Dispatcher;
                    if (d.CheckAccess())
                        manager.LoadModule(moduleInfo.ModuleName);
                    else
                        d.BeginInvoke((Action)delegate { manager.LoadModule(moduleInfo.ModuleName); });
                }
#else
                plugin = Assembly.LoadFrom(strModule);
                try
                {
                    types = plugin.GetTypes();
                }
                catch (Exception ex)
                {
                    continue;
                }
                foreach (Type t in types)
                {
                    //Check Module interface
                    if (t.GetInterface("IModule", true) == null)
                    {
                        continue;
                    }
                    IModule iModule = (IModule)t.InvokeMember(null,
                                  BindingFlags.DeclaredOnly |
                                  BindingFlags.Public | BindingFlags.NonPublic |
                                  BindingFlags.Instance | BindingFlags.CreateInstance,
                                  null, null, null);
                  

                    iModule.Initialize();
                }
#endif
                //reset temp.
                plugin = null;
                types = null;
            }
            Thread.Sleep(5000);
            CommonUILib.PrismDemoPubSubEvent<bool>.Instance.Publish(false);
        }

        private object CreateModuleInfo(Type type)
        {
            string moduleName = type.Name;

            var moduleAttribute = CustomAttributeData.GetCustomAttributes(type).FirstOrDefault(cad => cad.Constructor.DeclaringType.FullName == typeof(ModuleAttribute).FullName);

            if (moduleAttribute != null)
            {
                foreach (CustomAttributeNamedArgument argument in moduleAttribute.NamedArguments)
                {
                    string argumentName = argument.MemberInfo.Name;
                    if (argumentName == "ModuleName")
                    {
                        moduleName = (string)argument.TypedValue.Value;
                        break;
                    }
                }
            }

            ModuleInfo moduleInfo = new ModuleInfo(moduleName, type.AssemblyQualifiedName)
            {
                InitializationMode = InitializationMode.OnDemand,
                Ref = type.Assembly.CodeBase,
            };

            return moduleInfo;
        } 
    }
}
