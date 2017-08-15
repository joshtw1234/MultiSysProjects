using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ProfileTest
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainControl : UserControl, UtilityUILib.IModuleInterface
    {
        
        public MainControl()
        {
            InitializeComponent();
            this.Loaded += MainControl_Loaded;
        }

        private void MainControl_Loaded(object sender, RoutedEventArgs e)
        {
            ccProf.Content = new UCGAMEBASE();
            ccSQL.Content = new UCSQL();
        }
      
        #region Module Interface
        public string interfaceVersion
        {
            get
            {
                //throw new NotImplementedException();
                return string.Empty;
            }
        }

        public string moduleName
        {
            get
            {
                //throw new NotImplementedException();
                return "Prifile";
            }
        }

        public BitmapImage getModuleLogoImage()
        {
            throw new NotImplementedException();
        }

        public BitmapImage getModuleLogoImage2()
        {
            throw new NotImplementedException();
        }

        public int hide()
        {
            //throw new NotImplementedException();
            return 0;
        }

        public void initialize()
        {
            //throw new NotImplementedException();
        }

        public bool isPlatformSupported()
        {
            //throw new NotImplementedException();
            return true;
        }

        public int show()
        {
            //throw new NotImplementedException();
            return 0;
        }

        public void uninitialize()
        {
            //throw new NotImplementedException();
        }
        #endregion

    }
}
