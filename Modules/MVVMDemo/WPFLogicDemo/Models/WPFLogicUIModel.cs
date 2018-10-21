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
        const string ButtonLogic = "Logic";
        const string ButtonSTOCK = "STOCK";

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

        #region Commend & Private Methods
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
            else if (obj.Equals(ButtonLogic))
            {
                _pageLogicContent.ContentVisibility = Visibility.Visible;
                _pageSTOCKContent.ContentVisibility = Visibility.Collapsed;
            }
            else if (obj.Equals(ButtonSTOCK))
            {
                _pageLogicContent.ContentVisibility = Visibility.Collapsed;
                _pageSTOCKContent.ContentVisibility = Visibility.Visible;
            }
            else
            {
                messageText.MenuName = string.Empty;
            }
        }
        private ObservableCollection<IMenuItem> GetCommonButtons()
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
        private IMenuItem GetMessageText()
        {
            messageText = new MessageTextMenuItem()
            {
                MenuName = "Hello World!!!\n",
                MenuStyle = _localDic["MessageStyle"] as Style
            };
            return messageText;
        }
        private IMenuItem GetTextInput()
        {
            textInput = new MessageTextMenuItem()
            {
                MenuName = "Input Argus here"
            };
            return textInput;
        }
        private ObservableCollection<IMenuItem> GetSTOCKButtonCollection()
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
        #endregion

        #region InterFace
        public ObservableCollection<IMenuItem> GetPageItemsSource()
        {
            return new ObservableCollection<IMenuItem>()
            {
                new MenuItem()
                {
                    MenuName = "Logic Page",
                    MenuStyle = localDic["LogicBtnStyle"] as Style,
                    MenuData = ButtonLogic,
                    MenuCommand = OnCommonButtonClickEvent
                },
                new MenuItem()
                {
                    MenuName = "STOCK Page",
                    MenuStyle = localDic["LogicBtnStyle"] as Style,
                    MenuData = ButtonSTOCK,
                    MenuCommand = OnCommonButtonClickEvent
                }
            };
        }
        private LogicContentData _pageLogicContent;
        public LogicContentData GetLogicContentVM()
        {
            _pageLogicContent = new LogicContentData();
            _pageLogicContent.CommonButtonCollection = GetCommonButtons();
            _pageLogicContent.MessageText = GetMessageText();
            _pageLogicContent.TextInput = GetTextInput();
            return _pageLogicContent;

        }
        private STOCKContentData _pageSTOCKContent;
        public STOCKContentData GetSTOCKContentVM()
        {
            _pageSTOCKContent = new STOCKContentData();
            _pageSTOCKContent.STOCKButtonCollection = GetSTOCKButtonCollection();
            return _pageSTOCKContent;
        }
        #endregion

    }
}
