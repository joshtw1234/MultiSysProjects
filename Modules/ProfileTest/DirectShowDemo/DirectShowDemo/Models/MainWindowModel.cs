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
            if (obj.Equals("Apply"))
            {
                UWPAudioService.Instence.StartAudio();
            }
            else
            {
                UWPAudioService.Instence.StopAudio();
            }
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

        private IViewItem _displayMenuItem;

        public IViewItem GetDisplayMenuItem
        {
            get
            {
                return _displayMenuItem = new ViewItem()
                {
                    MenuName = "Hello Word!",
                    MenuStyle = localDic["MessageTextBlock"] as Style
                };
            }
        }

        public ObservableCollection<IViewItem> GetCommonButtons
        {
            get
            {
                return new ObservableCollection<IViewItem>()
                {
                    new ViewItem()
                    {
                        MenuName= "Cancel",
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnCommonButtonClickEvent,
                        MenuData = "Cancel"
                    },
                    new ViewItem()
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
            CoreAudioApiService.Instance.InitializeAudioDevice();
            var result = UWPAudioService.Instence.InitializeUWPAudio();

        }
    }
}
