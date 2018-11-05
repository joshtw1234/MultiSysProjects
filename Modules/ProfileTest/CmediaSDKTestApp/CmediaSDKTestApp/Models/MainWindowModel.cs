using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            int rev = NativeMethods.CMI_ConfLibUnInit();
        }

        private void InitialCaptureDevice(CMI_JackDeviceInfo jackInfo, CmediaCaptureFunctionPoint funPoint)
        {
            var rwData = new ZazuRWData() { JackInfo = jackInfo, CapturePropertyName = funPoint, ReadWrite = CMI_DriverRW.Read, WriteData = null };
            OMENREVData rev = BaseCmediaSDK.OMEN_PropertyControl(rwData);
            _micPage.DisplayText.MenuName += $"{rev.RevMessage}";
            rwData.ReadWrite = CMI_DriverRW.Write;
            uint setData = 0;
            if (int.Parse(rev.RevValue) == 0)
            {
                setData = 1;
            }
            rwData.WriteData = BitConverter.GetBytes(setData);
            rev = BaseCmediaSDK.OMEN_PropertyControl(rwData);
            _micPage.DisplayText.MenuName += $"{rev.RevMessage}";

            //rwData.ReadWrite = CMI_DriverRW.Read;
            //rev = BaseCmediaSDK.OMEN_PropertyControl(rwData);
            //_micPage.DisplayText.MenuName += $"{rev.RevMessage}";

        }

        private void InitialGetCMIDriverData(CMI_JackDeviceInfo jackInfo)
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
            ZazuRWData rwData = null;
            OMENREVData rev = null;
            int buffSize = 0;

            for (CmediaRenderFunctionPoint i = CmediaRenderFunctionPoint.DefaultDeviceControl; i <= CmediaRenderFunctionPoint.GetDriverVer; i++)
            {
                rwData = new ZazuRWData() { JackInfo = jackInfo, RenderPropertyName = i, ReadWrite = CMI_DriverRW.Read, WriteData = null };
                rev = BaseCmediaSDK.OMEN_PropertyControl(rwData);
                _micPage.DisplayText.MenuName += $"{rev.RevMessage}";
            }
            byte[] setByte = new byte[BaseCmediaSDK.CMI_BUFFER_SIZE];
            //OMEN_PropertyControl(jackInfo, BaseCmediaSDK.CMI_DefaultDeviceControl, CMI_DriverRW.Write, setByte);
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
            var rev = CmediaSDKHelper.Instance.Initialize(_micPage.DisplayText);

            Application.Current.MainWindow.Closing += MainWindow_Closing;
        }
    }
}
