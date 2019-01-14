using System;
using System.Collections.ObjectModel;
using System.Windows;
using MyStandardDLL.MVVMUtility;

namespace WPFAudioTest.Models
{
    class MainWindowModel : IMainWindowModel
    {
        private ResourceDictionary _localDic;
        private ResourceDictionary localDic
        {
            get
            {
                if (null == _localDic)
                {
                    _localDic = new ResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/WPFAudioTest;component/Styles/WPFAudioTestStyle.xaml", UriKind.RelativeOrAbsolute)
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
                return _displayMenuItem = new TextBoxItem()
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
                    new TextBoxItem()
                    {
                        MenuName= "Cancel",
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnCommonButtonClickEvent,
                        MenuData = "Cancel"
                    },
                    new TextBoxItem()
                    {
                        MenuName= "Apply",
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnCommonButtonClickEvent,
                        MenuData = "Apply"
                    }
                };
            }
        }

        private MyDelegateCommond<string> _onCommonButtonClickEvent;

        public MyDelegateCommond<string> OnCommonButtonClickEvent => _onCommonButtonClickEvent ?? (_onCommonButtonClickEvent = new MyDelegateCommond<string>(OnCommonButtonClick));

        private void OnCommonButtonClick(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
