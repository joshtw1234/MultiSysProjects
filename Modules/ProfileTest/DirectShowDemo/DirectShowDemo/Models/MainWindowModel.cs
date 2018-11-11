using System;
using System.Collections.ObjectModel;
using System.Windows;
using MVVMUtilities.Common;

namespace DirectShowDemo.Models
{
    class MainWindowModel : IMainWindowModel
    {
        private MyDelegateCommond<string> _onCommonButtonClickEvent;
        protected MyDelegateCommond<string> OnCommonButtonClickEvent => _onCommonButtonClickEvent ?? (_onCommonButtonClickEvent = new MyDelegateCommond<string>(OnCommonButtonClick));

        private void OnCommonButtonClick(string obj)
        {
            //throw new NotImplementedException();
        }

        private ResourceDictionary _localDic;
        private ResourceDictionary localDic
        {
            get
            {
                if (null == _localDic)
                {
                    _localDic = new ResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/DirectShowDemo;component/Styles/DirectShowDemoStyle.xaml", UriKind.RelativeOrAbsolute)
                    };
                }
                return _localDic;
            }
        }

        private IMenuItem _displayMenuItem;

        public IMenuItem GetDisplayMenuItem
        {
            get
            {
                return _displayMenuItem = new MenuItem()
                {
                    MenuName = "Hello Word!",
                    MenuStyle = localDic["MessageTextBlock"] as Style
                };
            }
        }

        public ObservableCollection<IMenuItem> GetCommonButtons
        {
            get
            {
                return new ObservableCollection<IMenuItem>()
                {
                    new MenuItem()
                    {
                        MenuName= "Cancel",
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnCommonButtonClickEvent,
                        MenuData = "Cancel"
                    },
                    new MenuItem()
                    {
                        MenuName= "Apply",
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnCommonButtonClickEvent,
                        MenuData = "Apply"
                    }
                };
            }
        }
        public void ModuleInitialize()
        {
            _displayMenuItem.MenuName += "\nModule Initialized";
            int rev = CoreAudioApiService.fnCoreAudioApisDll();

        }
    }
}
