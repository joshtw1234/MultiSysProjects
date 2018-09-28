using System;
using System.Collections.ObjectModel;
using System.Windows;
using UtilityUILib;

namespace WPFLogicDemo.Models
{
    class WPFLogicUIModel
    {
        protected const string ButtonStart = "Start";
        protected const string ButtonClear = "Clear";

        private ResourceDictionary _localDic;
        protected ResourceDictionary localDic
        {
            get
            {
                if (null == _localDic)
                {
                    _localDic = new ResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/WPFLogicDemo;component/Styles/WPFLogicStyle.xaml", UriKind.RelativeOrAbsolute)
                    };
                }
                return _localDic;
            }
        }

        private MyCommond<string> _onCommonButtonClickEvent;
        private MyCommond<string> OnCommonButtonClickEvent => _onCommonButtonClickEvent ?? (_onCommonButtonClickEvent = new MyCommond<string>(OnCommonButtonClick));
        protected virtual void OnCommonButtonClick(string obj)
        {
        }

        internal ObservableCollection<IMenuItem> GetCommonButtons()
        {
            return new ObservableCollection<IMenuItem>()
            {
                new MenuItem()
                {
                    MenuName = ButtonStart,
                    MenuStyle = localDic["LogicBtnStyle"] as Style,
                    MenuData = ButtonStart,
                    MenuCommand = OnCommonButtonClickEvent
                },
                new MenuItem()
                {
                    MenuName = ButtonClear,
                    MenuStyle = localDic["LogicBtnStyle"] as Style,
                    MenuData = ButtonClear,
                    MenuCommand = OnCommonButtonClickEvent
                }
            };
        }

        internal IMenuItem GetMessageText()
        {
            return new MessageTextMenuItem()
            {
                MenuName = "Hello World!!!\n",
                MenuStyle = _localDic["MessageStyle"] as Style
            };
        }
       
    }
}
