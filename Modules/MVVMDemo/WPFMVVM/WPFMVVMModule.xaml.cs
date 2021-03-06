﻿using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using UtilityUILib;

namespace WPFMVVM
{
    /// <summary>
    /// Interaction logic for WPFMVVMModule.xaml
    /// </summary>
    public partial class WPFMVVMModule : UserControl, IModuleInterface
    {
        public WPFMVVMModule()
        {
            InitializeComponent();
        }
        #region Module Interface
        public string interfaceVersion
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string moduleName
        {
            get
            {
                return "WPFMVVM";
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
            throw new NotImplementedException();
        }
        #endregion 
    }
}
