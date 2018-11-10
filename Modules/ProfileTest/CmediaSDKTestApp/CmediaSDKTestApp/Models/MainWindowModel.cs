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
            var rev = OMENZazuHelper.UnInitializeSDKAsync();
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
                var rev = OMENZazuHelper.SetAudioVolumeControl(new BaseVolumeControlStructure()
                {
                    IsMuted = Convert.ToInt32(muteBoxDataContext.SliderTitle.MenuChecked),
                    ChannelValues = new List<VolumeChannelSturcture>()
                });
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
                var rev = OMENZazuHelper.SetAudioVolumeControl(new BaseVolumeControlStructure()
                {
                    IsMuted = Convert.ToInt32(sliderDataContext.SliderTitle.MenuChecked),
                    ChannelValues = new List<VolumeChannelSturcture>()
                    {
                        new VolumeChannelSturcture()
                        {
                            ChannelIndex = VolumeChannel.Master,
                            ChannelValue = double.Parse(sliderDataContext.SliderValueStr.MenuName)
                        }
                    }
                });
            }
        }

        private void SliderInitialize()
        {
            //Audio
            //_micPage.SliderControls[0].SliderMaximum.MenuName = _audioVolumeControl.MaxValue.ToString();
            //_micPage.SliderControls[0].SliderMinimum.MenuName = _audioVolumeControl.MinValue.ToString();
            _micPage.SliderControls[0].SliderValueStr.MenuName = _audioVolumeControl.ScalarValue.ToString();
            //_micPage.SliderControls[0].SliderTickFrequency.MenuName = _audioVolumeControl.StepValue.ToString();
            _micPage.SliderControls[0].SliderTitle.MenuChecked = Convert.ToBoolean(_audioVolumeControl.IsMuted);
            //Microphone
            //_micPage.SliderControls[1].SliderMaximum.MenuName = _microphoneVolumeControl.MaxValue.ToString();
            //_micPage.SliderControls[1].SliderMinimum.MenuName = _microphoneVolumeControl.MinValue.ToString();
            _micPage.SliderControls[1].SliderValueStr.MenuName = _microphoneVolumeControl.ScalarValue.ToString();
            //_micPage.SliderControls[1].SliderTickFrequency.MenuName = _microphoneVolumeControl.StepValue.ToString();
            _micPage.SliderControls[1].SliderTitle.MenuChecked = Convert.ToBoolean(_microphoneVolumeControl.IsMuted);
        }

        private bool IsModuleInitialized = false;
        private bool IsReadDataFromDriver = false;
        private async Task ReadDataFromDriver()
        {
            _audioVolumeControl = await OMENZazuHelper.GetAudioVolumeControl();
            _micPage.DisplayText.MenuName += $"\nAudioVolumeControl get [{_audioVolumeControl.MaxValue}] [{_audioVolumeControl.MinValue}] [{_audioVolumeControl.ScalarValue}] [{_audioVolumeControl.IsMuted}]";
            _microphoneVolumeControl = await OMENZazuHelper.GetMicrophoneVolumeControl();
            _micPage.DisplayText.MenuName += $"\nMicrophoneVolumeControl get [{_microphoneVolumeControl.MaxValue}] [{_microphoneVolumeControl.MinValue}] [{_microphoneVolumeControl.ScalarValue}] [{_microphoneVolumeControl.IsMuted}]";
            SliderInitialize();
        }

        public void ModelInitialize()
        {
            
            Task.Factory.StartNew(async () => 
            {
                var rev = await OMENZazuHelper.InitializeSDKAsync(_micPage.DisplayText);
                if (rev != 0)
                {
                    _micPage.DisplayText.MenuName += "\nSDK initial failed";
                    return;
                }
                IsModuleInitialized = true;
                var resu = ReadDataFromDriver();
                //revOMEN = await CmediaSDKHelper.SetJackDeviceDataAsync(new OMENClientData() { ApiName = CmediaAPIFunctionPoint.DefaultDeviceControl.ToString(), SetValue= 0 });
                //revOMEN = await CmediaSDKHelper.GetJackDeviceDataAsync(new OMENClientData() { ApiName = CmediaAPIFunctionPoint.DefaultDeviceControl.ToString() });
                //revOMEN = await CmediaSDKHelper.GetJackDeviceDataAsync(new OMENClientData() { ApiName = CmediaRenderFunctionPoint.GetDriverVer.ToString() });
                //revOMEN = await CmediaSDKHelper.GetSurroundAsync(HPSurroundCommand.XEAR_SURR_HP_ENABLE);
                //revOMEN = await CmediaSDKHelper.GetSurroundAsync(HPSurroundCommand.XEAR_SURR_HP_MODE);
                //revOMEN = await CmediaSDKHelper.GetSurroundAsync(HPSurroundCommand.XEAR_SURR_HP_ROOM);

            });
            OMENZazuHelper.RegisterSDKCallbackFunction(OnCmediaSDKCallBack);
            Application.Current.MainWindow.Closing += MainWindow_Closing;
            _micPage.DisplayText.MenuName += $"\nCmediaRenderFunctionPoint get {Enum.GetNames(typeof(CmediaAPIFunctionPoint)).Length}";
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
