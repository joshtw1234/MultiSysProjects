using System;
using System.Collections.ObjectModel;
using System.Windows;
using UtilityUILib;
using WPFLogicDemo.Models;

namespace WPFLogicDemo.ViewModels
{
    class WPFLogicControlViewModel : BindAbleBases
    {
        const string ButtonClear = "Clear";

        IWPFLogicModel _wpflogicModel;

        ResourceDictionary _resDictionary;

        public WPFLogicControlViewModel(IWPFLogicModel wpflogicModel)
        {
            _wpflogicModel = wpflogicModel;
            _resDictionary = _wpflogicModel.GetLocalStyle();
            _commonButtonCollection = GetCommonButtons();
            _messageText = new MessageTextMenuItem()
            {
                MenuName = "Hello World!!!",
                MenuStyle = _resDictionary["MessageStyle"] as Style
            };
        }

        private ObservableCollection<IMenuItem> GetCommonButtons()
        {
            return new ObservableCollection<IMenuItem>()
            {
                new MenuItem()
                {
                    MenuName = ButtonClear,
                    MenuStyle = _resDictionary["LogicBtnStyle"] as Style,
                    MenuData = ButtonClear,
                    MenuCommand = new MyCommond<string>(OnCommonButtonClick)
                }
            };
        }

        private void OnCommonButtonClick(string obj)
        {
            if (obj.Equals(ButtonClear))
            {
                MessageText.MenuName = string.Empty;
            }
        }

        private ObservableCollection<IMenuItem> _commonButtonCollection;
        public ObservableCollection<IMenuItem> CommonButtonCollection
        {
            get
            {
                return _commonButtonCollection;
            }

            set
            {
                _commonButtonCollection = value;
                onPropertyChanged(this, "CommonButtonCollection");
            }
        }

        private IMenuItem _messageText;
        public IMenuItem MessageText
        {
            get
            {
                return _messageText;
            }

            set
            {
                _messageText = value;
                onPropertyChanged(this, "MessageText");
            }
        }
    }
}
