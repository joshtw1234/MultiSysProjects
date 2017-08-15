using ShareUtilityLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MainUWPApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
#if false
        void LoadModules()
        {
            //Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder
            //FileInfo fi = new FileInfo()
            //string modulePath = string.Format(@"{0}\Modules\UI\", AppDomain.CurrentDomain.BaseDirectory);
            string modulePath = string.Format(@"{0}\Modules\UI\", Directory.GetCurrentDirectory());
            string[] strModuleLst = Directory.GetFiles(modulePath, "*.dll", SearchOption.TopDirectoryOnly);
            Assembly plugin;
            Type[] types;
            foreach (string strModule in strModuleLst)
            {
                //C# load dll method
                plugin = Assembly.LoadFrom(strModule);
                types = plugin.GetTypes();

                foreach (Type t in types)
                {
                    //Check Module interface
                    if (t.GetInterface("IModuleInterface", true) == null)
                    {
                        continue;
                    }
                    //Check is main user control
                    if (!typeof(System.Windows.Controls.UserControl).IsAssignableFrom(t))
                    {
                        continue;
                    }
                    //Check main user control name
                    if (t.Name != "MainControl")
                    {
                        continue;
                    }
                    IModuleInterface iModule = (IModuleInterface)t.InvokeMember(null,
                                   BindingFlags.DeclaredOnly |
                                   BindingFlags.Public | BindingFlags.NonPublic |
                                   BindingFlags.Instance | BindingFlags.CreateInstance,
                                   null, null, null);
                    //Check module is support and load into memory
                    if (iModule.isPlatformSupported())
                    {

                        Button btnModule = new Button();
                        btnModule.Name = string.Format("btn{0}", iModule.moduleName);
                        btnModule.Content = iModule.moduleName;
                        spModules.Children.Add(btnModule);
                        btnModule.Click += BtnModule_Click; ;
                        UserControl uc = (UserControl)iModule;
                        uc.Name = iModule.moduleName;
                        uc.Visibility = Visibility.Collapsed;
                        gdModlueUI.Children.Add(uc);
                    }

                }
                //reset temp.
                plugin = null;
                types = null;
            }
            //Use Update as main module.
            ShowModule("Update");
        }
#endif

        private void BtnModule_Click(object sender, RoutedEventArgs e)
        {
            ShowModule((sender as Button).Content.ToString());
        }

        /// <summary>
        /// Show the module UI.
        /// </summary>
        /// <param name="strModuleName"></param>
        void ShowModule(string strModuleName)
        {
            foreach (UIElement uie in gdModlueUI.Children)
            {
                uie.Visibility = Visibility.Collapsed;
            }
            UserControl uc = gdModlueUI.Children.Cast<UserControl>().FirstOrDefault(x => x.Name.Equals(strModuleName));
            if (uc != null)
            {
                uc.Visibility = Visibility.Visible;
            }
        }
    }
}
