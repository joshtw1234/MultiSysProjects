using System;
using System.Collections.ObjectModel;
using System.Windows;
using CmediaSDKTestApp.BaseModels;

namespace CmediaSDKTestApp.Models
{
    class MainWindowModel : MainWindowLogicModel, IMainWindowModel
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
                        Source = new Uri("pack://application:,,,/CmediaSDKTestApp;component/Styles/CmediaSDKTestAppStyle.xaml", UriKind.RelativeOrAbsolute)
                    };
                }
                return _localDic;
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var rev = CmediaSDKHelper.UnInitializeSDKAsync();
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

        public ObservableCollection<IMenuItem> GetContentPages
        {
            get
            {
                _micPage = new MicPageContentModel()
                {
                    MenuName = ButtonStrings.Microphone.ToString(),
                    MenuStyle = localDic["MICContentPageStyle"] as Style,
                    DisplayText = new BindAbleMenuItem()
                    {
                        MenuName = "Message here"
                    },
                    
                    SliderControls = new ObservableCollection<IMenuItem>()
                    {
#if true
                        new BindAbleMenuItem()
                        {
                            MenuName = "Enable Magic",
                            MenuStyle = localDic["BaseToggleButton"] as Style,
                            MenuData = ButtonStrings.EnableMagic.ToString(),
                            MenuCommand = OnCommonButtonClickEvent
                        },
                        new BindAbleMenuItem()
                        {
                            MenuName = "Enable ECHO",
                            MenuStyle = localDic["BaseToggleButton"] as Style,
                            MenuData = ButtonStrings.EnableEcho.ToString(),
                            MenuCommand = OnCommonButtonClickEvent
                        },
#else
                        new HorzSliderControlViewModel()
                            {
                                SliderValueStr = new MenuItem()
                                {
                                    MenuName = "100",
                                },
                                SlideUnitStr = new  MenuItem()
                                {
                                    MenuName = "v",
                                },
                                SliderTitle = new MenuItem()
                                {
                                    MenuName = "Slider Title",
                                },
                                SliderMaximum = new MenuItem()
                                {
                                    MenuName = "200"
                                },
                                SliderMinimum = new MenuItem()
                                {
                                    MenuName = "0"
                                },
                                SliderTickFrequency = new MenuItem()
                                {
                                    MenuName = "5"
                                }
                            },
                            new HorzSliderControlViewModel()
                            {
                                SliderValueStr = new MenuItem()
                                {
                                    MenuName = "100",
                                },
                                SlideUnitStr = new  MenuItem()
                                {
                                    MenuName = "v",
                                },
                                SliderTitle = new MenuItem()
                                {
                                    MenuName = "Slider Title",
                                },
                                SliderMaximum = new MenuItem()
                                {
                                    MenuName = "200"
                                },
                                SliderMinimum = new MenuItem()
                                {
                                    MenuName = "0"
                                },
                                SliderTickFrequency = new MenuItem()
                                {
                                    MenuName = "5"
                                }
                            },
                            new HorzSliderControlViewModel()
                            {
                                SliderValueStr = new MenuItem()
                                {
                                    MenuName = "100",
                                },
                                SlideUnitStr = new  MenuItem()
                                {
                                    MenuName = "v",
                                },
                                SliderTitle = new MenuItem()
                                {
                                    MenuName = "Slider Title",
                                },
                                SliderMaximum = new MenuItem()
                                {
                                    MenuName = "200"
                                },
                                SliderMinimum = new MenuItem()
                                {
                                    MenuName = "0"
                                },
                                SliderTickFrequency = new MenuItem()
                                {
                                    MenuName = "5"
                                }
                            }
#endif
                    }

                };
                _contentpages = new ObservableCollection<IMenuItem>()
                {
                    new BasePageContentModel()
                    {
                        MenuName = ButtonStrings.Equaliser.ToString(),
                        MenuStyle = localDic["EQContentPageStyle"] as Style,
                        MenuVisibility = true,
                        MenuImage = "/CmediaSDKTestApp;component/Assets/TestAsset1.jpg"
                    },
                    new BasePageContentModel()
                    {
                        MenuName = ButtonStrings.Lighting.ToString(),
                        MenuStyle = localDic["BaseContentPageStyle"] as Style,
                    },
                    new BasePageContentModel()
                    {
                        MenuName = ButtonStrings.Cooling.ToString(),
                        MenuStyle = localDic["BaseContentPageStyle"] as Style,
                    },
                };
                _contentpages.Add(_micPage);
                return _contentpages;
            }
        }

        public void ModelInitialize()
        {
            var rev = CmediaSDKHelper.InitializeSDKAsync(_micPage.DisplayText);
            int cRev = CmediaSDKHelper.RegisterSDKCallbackFunction(OnCmediaSDKCallBack);

            Application.Current.MainWindow.Closing += MainWindow_Closing;
        }

        private void OnCmediaSDKCallBack(int type, int id, int componentType, ulong eventId)
        {
            //throw new NotImplementedException();
        }
    }
}
