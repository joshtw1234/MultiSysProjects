using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
            var rev = OMENCmediaSDK.OMENSDK.OMENZazuHelper.UnInitializeSDKAsync();
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
#if false
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
#endif
                var defSlider = new HorzSliderControlModel()
                {
                    SliderName = "Audio",
                    SliderValueStr = new BindAbleMenuItem()
                    {
                        MenuName = "40"
                    },
                    SlideUnitStr = new MenuItem()
                    {
                        MenuName = "v",
                    },
                    SliderTitle = new BindAbleMenuItem()
                    {
                        MenuName = "Audio Volume Control",
                        MenuData = "Is Mute"
                    },
                    SliderMaximum = new MenuItem()
                    {
                        MenuName = "1"
                    },
                    SliderMinimum = new MenuItem()
                    {
                        MenuName = "0"
                    },
                    SliderTickFrequency = new MenuItem()
                    {
                        MenuName = "0.01"
                    },
                    SliderValueChangeCommand = OnSliderValueChangeCommand,
                    MuteBoxCheckedCommand = OnMuteBoxCheckedCommand
                };
                var secSlider = new HorzSliderControlModel()
                {
                    SliderName = "Microphone",
                    SliderValueStr = new BindAbleMenuItem()
                    {
                        MenuName = "40"
                    },
                    SlideUnitStr = new MenuItem()
                    {
                        MenuName = "v",
                    },
                    SliderTitle = new BindAbleMenuItem()
                    {
                        MenuName = "Microphone Volume Control",
                        MenuData = "Is Mute"
                    },
                    SliderMaximum = new MenuItem()
                    {
                        MenuName = "1"
                    },
                    SliderMinimum = new MenuItem()
                    {
                        MenuName = "0"
                    },
                    SliderTickFrequency = new MenuItem()
                    {
                        MenuName = "0.01"
                    },
                    SliderValueChangeCommand = OnSliderValueChangeCommand,
                    MuteBoxCheckedCommand = OnMuteBoxCheckedCommand
                };

                _micPage = new MicPageContentModel()
                {
                    MenuName = ButtonStrings.Microphone.ToString(),
                    MenuStyle = localDic["MICContentPageStyle"] as Style,
                    DisplayText = new BindAbleMenuItem()
                    {
                        MenuName = "Message here"
                    },

                    SliderControls = new ObservableCollection<HorzSliderControlModel>()
                };
                _micPage.SliderControls.Add(defSlider);
                _micPage.SliderControls.Add(secSlider);
                _contentpages = new ObservableCollection<IMenuItem>()
                {
                    new BasePageContentModel()
                    {
                        MenuName = ButtonStrings.Equaliser.ToString(),
                        MenuStyle = localDic["EQContentPageStyle"] as Style,
                        MenuVisibility = true,
                        //MenuImage = "/CmediaSDKTestApp;component/Assets/TestAsset1.jpg"
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

        protected override void OnMuteBoxChecked(RoutedEventArgs obj)
        {
            if (IsReadDataFromDriver)
            {
                IsReadDataFromDriver = !IsReadDataFromDriver;
                return;
            }
            HorzSliderControlModel muteBoxDataContext = (obj.Source as System.Windows.Controls.CheckBox).DataContext as HorzSliderControlModel;
            if (muteBoxDataContext.SliderName.Equals("Audio"))
            {
                var rev = OMENCmediaSDK.OMENSDK.OMENZazuHelper.SetAudioMuteControl(Convert.ToInt32(muteBoxDataContext.SliderTitle.MenuChecked));
            }
        }

        protected override void OnSliderValueChanged(RoutedPropertyChangedEventArgs<double> obj)
        {
            if (IsReadDataFromDriver)
            {
                IsReadDataFromDriver = !IsReadDataFromDriver;
                return;
            }
            HorzSliderControlModel sliderDataContext = (obj.Source as System.Windows.Controls.Slider).DataContext as HorzSliderControlModel;
            if (sliderDataContext.SliderName.Equals("Audio"))
            {
                var rev = OMENCmediaSDK.OMENSDK.OMENZazuHelper.SetAudioVolumeScalarControl(new List<OMENCmediaSDK.OMENSDK.Structures.VolumeChannelSturcture>()
                {
                    new OMENCmediaSDK.OMENSDK.Structures.VolumeChannelSturcture()
                    {
                        ChannelIndex = OMENCmediaSDK.OMENSDK.Structures.OMENVolumeChannel.Master,
                        ChannelValue = float.Parse(sliderDataContext.SliderValueStr.MenuName)
                    }
                });
            }
        }

        private void SliderInitialize()
        {
            //Audio
            _micPage.SliderControls[0].SliderValueStr.MenuName = _audioVolumeControl.ScalarValue.ToString();
            _micPage.SliderControls[0].SliderTitle.MenuChecked = Convert.ToBoolean(_audioVolumeControl.IsMuted);
            //Microphone
            _micPage.SliderControls[1].SliderValueStr.MenuName = _microphoneVolumeControl.ScalarValue.ToString();
            _micPage.SliderControls[1].SliderTitle.MenuChecked = Convert.ToBoolean(_microphoneVolumeControl.IsMuted);
        }

        private bool IsModuleInitialized = false;
        private bool IsReadDataFromDriver = false;
        private async Task ReadDataFromDriver()
        {
            
            _audioVolumeControl = await OMENCmediaSDK.OMENSDK.OMENZazuHelper.GetAudioVolumeControl();
            _micPage.DisplayText.MenuName += $"\nAudioVolumeControl get [{_audioVolumeControl.MaxValue}] [{_audioVolumeControl.MinValue}] [{_audioVolumeControl.ScalarValue}] [{_audioVolumeControl.IsMuted}]";
            _microphoneVolumeControl = await OMENCmediaSDK.OMENSDK.OMENZazuHelper.GetMicrophoneVolumeControl();
            _micPage.DisplayText.MenuName += $"\nMicrophoneVolumeControl get [{_microphoneVolumeControl.MaxValue}] [{_microphoneVolumeControl.MinValue}] [{_microphoneVolumeControl.ScalarValue}] [{_microphoneVolumeControl.IsMuted}]";
            SliderInitialize();
        }

        public void ModelInitialize()
        {
            Task.Factory.StartNew(async () => 
            {
                var rev = await OMENCmediaSDK.OMENSDK.OMENZazuHelper.InitializeSDKAsync();
                if (rev != 0)
                {
                    _micPage.DisplayText.MenuName += "\nSDK initial failed";
                    return;
                }
                IsModuleInitialized = true;
                await ReadDataFromDriver();

            });
            OMENCmediaSDK.OMENSDK.OMENZazuHelper.RegisterSDKCallbackFunction(OnCmediaSDKCallBack);
            OMENCmediaSDK.OMENSDK.OMENZazuHelper.RegisterSDKCallbackFunction(On2CmediaSDKCallBack);
            Application.Current.MainWindow.Closing += MainWindow_Closing;
            //_micPage.DisplayText.MenuName += $"\nCmediaRenderFunctionPoint get {Enum.GetNames(typeof(CmediaAPIFunctionPoint)).Length}";
        }

        private void On2CmediaSDKCallBack(int type, int id, int componentType, ulong eventId)
        {
            _micPage.DisplayText.MenuName += $"\n On2CmediaSDKCallBack {type} {id} {componentType} {eventId}";
            if (IsModuleInitialized)
            {
                var resu = ReadDataFromDriver();
                IsReadDataFromDriver = true;
            }
        }

        private void OnCmediaSDKCallBack(int type, int id, int componentType, ulong eventId)
        {
            _micPage.DisplayText.MenuName += $"\n OnCmediaSDKCallBack {type} {id} {componentType} {eventId}";
            if (IsModuleInitialized)
            {
                var resu = ReadDataFromDriver();
                IsReadDataFromDriver = true;
            }
        }
    }
}
