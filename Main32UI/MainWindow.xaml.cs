using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UtilityUILib;

namespace Main32UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            Utilities.InitializeLogFile(CommonUIConsts.LogUtilityFileName);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //UtilityUILib.Db_API db = new UtilityUILib.Db_API();
            //List<string> tableLst = new List<string>();
            //tableLst.AddRange(db.GetDbTable("Josh.db"));
            LoadModules();
        }
        /// <summary>
        /// Search *.dll under Modules\UI\ folder and load into Main process.
        /// </summary>
        void LoadModules()
        {
            //FileInfo fi = new FileInfo()
            string modulePath = string.Format(@"{0}Modules\UI\", AppDomain.CurrentDomain.BaseDirectory);
            string[] strModuleLst = Directory.GetFiles(modulePath, "*.dll", SearchOption.TopDirectoryOnly);
            Assembly plugin;
            Type[] types = null;
            foreach (string strModule in strModuleLst)
            {
                //C# load dll method
                Utilities.Logger(CommonUIConsts.LogUtilityFileName, strModule);
                plugin = Assembly.LoadFrom(strModule);
                Utilities.Logger(CommonUIConsts.LogUtilityFileName, "0");
                try
                {
                    types = plugin.GetTypes();
                }
                catch(Exception ex)
                {
                    Utilities.Logger(CommonUIConsts.LogUtilityFileName, $"Error {ex.Message}");
                    continue;
                }
                Utilities.Logger(CommonUIConsts.LogUtilityFileName, "1");
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
                    //if (t.Name != "MainControl")
                    //{
                    //    continue;
                    //}
                    IModuleInterface iModule = (IModuleInterface)t.InvokeMember(null,
                                   BindingFlags.DeclaredOnly |
                                   BindingFlags.Public | BindingFlags.NonPublic |
                                   BindingFlags.Instance | BindingFlags.CreateInstance,
                                   null, null, null);
                    Utilities.Logger(CommonUIConsts.LogUtilityFileName, "2");
                    //Check module is support and load into memory
                    if (iModule.isPlatformSupported())
                    {

                        Button btnModule = new Button();
                        btnModule.Name = $"btn{iModule.moduleName}";
                        btnModule.Content = iModule.moduleName;
                        spModules.Children.Add(btnModule);
                        btnModule.Click += BtnModule_Click;
                        UserControl uc = (UserControl)iModule;
                        uc.Name = iModule.moduleName;
                        uc.Visibility = Visibility.Hidden;
                        gdModlueUI.Children.Add(uc);
                        iModule.initialize();
                        Utilities.Logger(CommonUIConsts.LogUtilityFileName, "3");
                    }

                }
                //reset temp.
                plugin = null;
                types = null;
            }
            //Use Update as main module.
            ShowModule("Update");
        }

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
            foreach(UIElement uie in gdModlueUI.Children)
            {
                uie.Visibility = Visibility.Hidden;
            }
            UserControl uc = gdModlueUI.Children.Cast<UserControl>().FirstOrDefault(x => x.Name.Equals(strModuleName));
            if (uc != null)
            {
                uc.Visibility = Visibility.Visible;
            }
        }
    }
}
