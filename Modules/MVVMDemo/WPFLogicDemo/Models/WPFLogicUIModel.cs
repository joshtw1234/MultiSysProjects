using System;
using System.Collections.ObjectModel;
using System.Windows;
using UtilityUILib;

namespace WPFLogicDemo.Models
{
    /// <summary>
    /// GUI Model Class contain GUI properties for Binding
    /// </summary>
    class WPFLogicUIModel : WPFLogicModel, IWPFLogicModel
    {
        const string ButtonStart = "Start";
        const string ButtonClear = "Clear";

        #region UI properties for Binding
        private ResourceDictionary _localDic;
        private ResourceDictionary localDic
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

        private IMenuItem messageText;

        private IMenuItem textInput;
        #endregion


        #region Commend
        private MyCommond<string> _onCommonButtonClickEvent;
        private MyCommond<string> OnCommonButtonClickEvent => _onCommonButtonClickEvent ?? (_onCommonButtonClickEvent = new MyCommond<string>(OnCommonButtonClick));
        private void OnCommonButtonClick(string obj)
        {
            if (obj.Equals(ButtonStart))
            {
                //SetAsyncAwaitAooRun(messageText);
                //SetAsyncAwaitBooRun(messageText);
                //SetAsyncAwaitCooRun(messageText);
                var revResult = GetDriverVersion(textInput.MenuName, messageText);
                messageText.MenuName += $"\n{revResult}";
            }
            else
            {
                messageText.MenuName = string.Empty;
            }
        }
        #endregion

        #region InterFace
        public ObservableCollection<IMenuItem> GetCommonButtons()
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

        public IMenuItem GetMessageText()
        {
            messageText = new MessageTextMenuItem()
            {
                MenuName = "Hello World!!!\n",
                MenuStyle = _localDic["MessageStyle"] as Style
            };
            return messageText;
        }

        public IMenuItem GetTextInput()
        {
            textInput = new MessageTextMenuItem()
            {
                MenuName = "Input Argus here"
            };
            return textInput;
        }
        #endregion

    }
}
