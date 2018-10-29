using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CmediaSDKTestApp.BaseModels;

namespace CmediaSDKTestApp.Models
{
    class MainWindowModel : IMainWindowModel
    {
        enum ButtonStrings
        {
            Equaliser,
            Microphone,
            Lighting,
            Cooling,
            Cancle,
            Apply
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
                        Source = new Uri("pack://application:,,,/CmediaSDKTestApp;component/Styles/MultiAccessoryTestStyle.xaml", UriKind.RelativeOrAbsolute)
                    };
                }
                return _localDic;
            }
        }

        private ObservableCollection<BasePageContentViewModel> _contentpages;

        private MyCommond<string> _onCommonButtonClickEvent;
        private MyCommond<string> OnCommonButtonClickEvent => _onCommonButtonClickEvent ?? (_onCommonButtonClickEvent = new MyCommond<string>(OnCommonButtonClick));
        private MyCommond<string> _onPageButtonClickEvent;
        private MyCommond<string> OnPageButtonClickEvent => _onPageButtonClickEvent ?? (_onPageButtonClickEvent = new MyCommond<string>(OnPageButtonClick));

        private void OnPageButtonClick(string obj)
        {
            var btn = (ButtonStrings)Enum.Parse(typeof(ButtonStrings), obj);
            foreach(IMenuItem page in _contentpages)
            {
                page.MenuVisibility = false;
            }
            _contentpages.FirstOrDefault(x => x.MenuName.Equals(obj)).MenuVisibility = true;
            switch (btn)
            {
                case ButtonStrings.Microphone:
                    break;
                case ButtonStrings.Equaliser:
                    break;
                case ButtonStrings.Lighting:
                    break;
                case ButtonStrings.Cooling:
                    break;
            }
        }

        private void OnCommonButtonClick(string obj)
        {
            var btn = (ButtonStrings)Enum.Parse(typeof(ButtonStrings), obj);
            switch(btn)
            {
                case ButtonStrings.Apply:
                    break;
                case ButtonStrings.Cancle:
                    break;
            }
        }

        public ObservableCollection<IMenuItem> GetPageButtons
        {
            get
            {
                return new ObservableCollection<IMenuItem>()
                {
                    new MenuItem()
                    {
                        MenuName = ButtonStrings.Equaliser.ToString(),
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnPageButtonClickEvent,
                        MenuData = ButtonStrings.Equaliser.ToString()
                    },
                    new MenuItem()
                    {
                        MenuName = ButtonStrings.Microphone.ToString(),
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnPageButtonClickEvent,
                        MenuData = ButtonStrings.Microphone.ToString()
                    },
                    new MenuItem()
                    {
                        MenuName = ButtonStrings.Lighting.ToString(),
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnPageButtonClickEvent,
                        MenuData = ButtonStrings.Lighting.ToString()
                    },
                    new MenuItem()
                    {
                        MenuName = ButtonStrings.Cooling.ToString(),
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnPageButtonClickEvent,
                        MenuData = ButtonStrings.Cooling.ToString()
                    }
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
                        MenuName = ButtonStrings.Cancle.ToString(),
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnCommonButtonClickEvent,
                        MenuData = ButtonStrings.Cancle.ToString()
                    },
                    new MenuItem()
                    {
                        MenuName = ButtonStrings.Apply.ToString(),
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnCommonButtonClickEvent,
                        MenuData = ButtonStrings.Apply.ToString()
                    }
                };
            }
        }

        public ObservableCollection<BasePageContentViewModel> GetContentPages
        {
            get
            {
                return _contentpages = new ObservableCollection<BasePageContentViewModel>()
                {
                    new BasePageContentViewModel()
                    {
                        MenuName = ButtonStrings.Equaliser.ToString(),
                        MenuStyle = localDic["EQContentPageStyle"] as Style,
                        MenuVisibility = true,
                    },
                    new BasePageContentViewModel()
                    {
                        MenuName = ButtonStrings.Microphone.ToString(),
                        MenuStyle = localDic["BaseContentPageStyle"] as Style,
                    },
                    new BasePageContentViewModel()
                    {
                        MenuName = ButtonStrings.Lighting.ToString(),
                        MenuStyle = localDic["BaseContentPageStyle"] as Style,
                    },
                    new BasePageContentViewModel()
                    {
                        MenuName = ButtonStrings.Cooling.ToString(),
                        MenuStyle = localDic["BaseContentPageStyle"] as Style,
                    },
                };
            }
        }
    }
}
