using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
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
                InitialCMIDriver(jackInfo);
            }
            _micPage.DisplayText.MenuName += $"\n{msg}";
        }

        private void InitialCMIDriver(CMI_JackDeviceInfo jackInfo)
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
            OMEN_PropertyControl(jackInfo, BaseCmediaSDK.CMI_DefaultDeviceControl);
            OMEN_PropertyControl(jackInfo, BaseCmediaSDK.CMI_Enable_KEYSHIFT_GFX);
            byte[] setByte = new byte[BaseCmediaSDK.CMI_BUFFER_SIZE];
            OMEN_PropertyControl(jackInfo, BaseCmediaSDK.CMI_DefaultDeviceControl, CMI_DriverRW.Write, setByte);
        }

        private void OMEN_PropertyControl(CMI_JackDeviceInfo jackInfo, string propertyName, CMI_DriverRW readWrite = CMI_DriverRW.Read, byte[] setData = null)
        {
            // Allocate a Cmedia standard Array.
            byte[] devBvalue = new byte[BaseCmediaSDK.CMI_BUFFER_SIZE];
            if (readWrite == CMI_DriverRW.Write)
            {
                devBvalue = setData;
            }
            // Allocate a memory buffer (that can be accessed and modified by unmanaged code)
            // to store values from the devBvalue array.
            IntPtr pdevValue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * devBvalue.Length);

            // Copy values from devBvalue to this buffer (i.e. pdevValue).
            Marshal.Copy(devBvalue, 0, pdevValue, devBvalue.Length);

            // Allocate a GCHandle in order to allocate an IntPtr
            // that stores the memory address of pdevValue.
            GCHandle gchDevValue = GCHandle.Alloc(pdevValue, GCHandleType.Pinned);
            // Use GCHandle.AddrOfPinnedObject() to obtain a pointer 
            // to a pointer to the byte array of pdevValue.
            // It is devValue that will be passed to the API.
            IntPtr devValue = gchDevValue.AddrOfPinnedObject();

            byte[] devExtraBvalue = new byte[BaseCmediaSDK.CMI_BUFFER_SIZE];
            IntPtr pdevExtraVlue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * devExtraBvalue.Length);
            Marshal.Copy(devExtraBvalue, 0, pdevExtraVlue, devExtraBvalue.Length);
            GCHandle gch = GCHandle.Alloc(pdevExtraVlue, GCHandleType.Pinned);
            IntPtr devExtraValue = gch.AddrOfPinnedObject();

            // Call the CMI_PropertyControl() API.
            // The CMI_PropertyControl() API will not
            // change the value of devValue. 
            int rev = NativeMethods.CMI_PropertyControl(jackInfo.m_devInfo, propertyName, devValue, devExtraValue, readWrite);
            _micPage.DisplayText.MenuName += $"\nCMI_PropertyControl return {rev}";
            if (0 == rev)
            {
                // We must now find a way to dereference the memory address
                // contained inside devValue.

                // Declare an array (of one single value) of IntPtr.
                IntPtr[] revPtr = new IntPtr[1];
                // Copy the value contained inside devValue
                // to revPtr.
                Marshal.Copy(devValue, revPtr, 0, 1);

                // Allocate a new byte array to be filled with 
                // values from the array pointed to by revPtr[0]
                byte[] NewByteArray = new byte[BaseCmediaSDK.CMI_BUFFER_SIZE];

                // Copy the byte array values pointed to by revPtr[0]
                // to NewByteArray.
                Marshal.Copy(revPtr[0], NewByteArray, 0, NewByteArray.Length);
                string revData = System.Text.Encoding.ASCII.GetString(NewByteArray).TrimEnd('\0');
                _micPage.DisplayText.MenuName += $"\n{propertyName} {readWrite} OK {revData}";
            }
            gchDevValue.Free();
            gch.Free();
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
