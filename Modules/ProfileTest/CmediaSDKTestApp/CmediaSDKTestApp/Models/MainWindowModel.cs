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
                        Source = new Uri("pack://application:,,,/CmediaSDKTestApp;component/Styles/CmediaSDKTestAppStyle.xaml", UriKind.RelativeOrAbsolute)
                    };
                }
                return _localDic;
            }
        }

        MicPageContentModel _micPage;
        CmediaSDKCallback _cmediaSDKCallback;

        internal void ModelInitialize()
        {
            CMI_DEVICEINFO devInfo;
            CMI_JackDeviceInfo jackInfo = new CMI_JackDeviceInfo();
            string msg = "Found Device";
            uint devCount = 0;
            int rev = NativeMethods.CMI_ConfLibInit();
            rev = NativeMethods.CMI_CreateDeviceList();
            rev = NativeMethods.CMI_GetDeviceCount(CMI_DeviceType.Render, ref devCount);
            if (0 == devCount)
            {
                msg = "Device not found!!";
            }
            else
            {
                for(int i = 0; i < devCount; i++)
                {
                    rev = NativeMethods.CMI_GetDeviceById(CMI_DeviceType.Render, i, out devInfo);
                    switch(devInfo.DeviceState)
                    {
                        case CMI_DeviceState.Active:
                            jackInfo.m_devInfo = devInfo;
                            msg += $" JackType [{jackInfo.m_devInfo.JackType}]";
                            break;
                        default:
                            msg = $"Device State {devInfo.DeviceState}";
                            break;
                    }
                };
                InitialGetCMIDriverData(jackInfo);
                _cmediaSDKCallback = OnCmediaSDKCallback;
                rev = NativeMethods.CMI_RegisterCallbackFunction(_cmediaSDKCallback, IntPtr.Zero);
                msg += $"\nRegisterCallback return {rev}";
            }
            _micPage.DisplayText.MenuName += $"\n{msg}";
            Application.Current.MainWindow.Closing += MainWindow_Closing;
        }

        private void OnCmediaSDKCallback(int type, int id, int componentType, ulong eventId)
        {
            //throw new NotImplementedException();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int rev = NativeMethods.CMI_ConfLibUnInit();
        }

        private async void InitialGetCMIDriverData(CMI_JackDeviceInfo jackInfo)
        {
            #region SDK sample code
#if false
            throw new NotImplementedException();
            memset(m_pGetValueBuffer, 0, STATIC_BUFFER_SIZE);
            nResult = CMI_PropertyControl(g_JackDevRender.m_devInfo, _T("Enable_KEYSHIFT_GFX"), (void**)&m_pGetValueBuffer, NULL, DRIVER_READ_FLAG);
            if (nResult != 0)
            {
                memset(m_pSetValueBuffer, 0, STATIC_BUFFER_SIZE);
                DWORD dwValue = 0;
                memcpy(m_pSetValueBuffer, &dwValue, sizeof(DWORD));
                nResult = CMI_PropertyControl(g_JackDevRender.m_devInfo, _T("Enable_KEYSHIFT_GFX"), (void**)&m_pSetValueBuffer, NULL, DRIVER_WRITE_FLAG);
            }
#endif
            #endregion
            ZazuRWData rwData = new ZazuRWData() { JackInfo = jackInfo, PropertyName = CMI_FunctinoPoint.DefaultDeviceControl, ReadWrite = CMI_DriverRW.Read, WriteData = null };
            OMENREVData rev = null;
            for(CMI_FunctinoPoint i = CMI_FunctinoPoint.DefaultDeviceControl; i<= CMI_FunctinoPoint.MagicVoice_Selection; i++)
            {
                rwData.PropertyName = i;
                rev = await BaseCmediaSDK.OMEN_PropertyControl(rwData);
                _micPage.DisplayText.MenuName += $"{rev.RevMessage}";
            }
            byte[] setByte = new byte[BaseCmediaSDK.CMI_BUFFER_SIZE];
            //OMEN_PropertyControl(jackInfo, BaseCmediaSDK.CMI_DefaultDeviceControl, CMI_DriverRW.Write, setByte);
        }


        private ObservableCollection<IMenuItem> _contentpages;

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
    }
}
