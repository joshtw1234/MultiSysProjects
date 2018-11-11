using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using DirectX.Capture;
using MVVMUtilities.Common;
using PCVolumeControl.Constants;
using PCVolumeControl.Win32;
using static PCVolumeControl.Structs.VolumeStructs;

namespace DirectShowDemo.Models
{
    class MainWindowModel : IMainWindowModel
    {
        private MyDelegateCommond<string> _onCommonButtonClickEvent;
        protected MyDelegateCommond<string> OnCommonButtonClickEvent => _onCommonButtonClickEvent ?? (_onCommonButtonClickEvent = new MyDelegateCommond<string>(OnCommonButtonClick));

        private void OnCommonButtonClick(string obj)
        {
            //throw new NotImplementedException();
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
                        Source = new Uri("pack://application:,,,/DirectShowDemo;component/Styles/DirectShowDemoStyle.xaml", UriKind.RelativeOrAbsolute)
                    };
                }
                return _localDic;
            }
        }

        private IMenuItem _displayMenuItem;

        public IMenuItem GetDisplayMenuItem
        {
            get
            {
                return _displayMenuItem = new MenuItem()
                {
                    MenuName = "Hello Word!",
                    MenuStyle = localDic["MessageTextBlock"] as Style
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
                        MenuName= "Cancel",
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnCommonButtonClickEvent,
                        MenuData = "Cancel"
                    },
                    new MenuItem()
                    {
                        MenuName= "Apply",
                        MenuStyle = localDic["BaseButtonStyle"] as Style,
                        MenuCommand = OnCommonButtonClickEvent,
                        MenuData = "Apply"
                    }
                };
            }
        }
        private Filters filters = new Filters();
        public void ModuleInitialize()
        {
            _displayMenuItem.MenuName += "\nModule Initialized";
            _displayMenuItem.MenuName += $"\nFilter Video Input {filters.VideoInputDevices.Count} Audio Input {filters.AudioInputDevices.Count}";
            StartPCVolumeControl();

        }
        private void StartPCVolumeControl()
        {
            List<MixerCaps> lstMixerCaps = new List<MixerCaps>();
            List<MIXERLINE> lstMixerLine = new List<MIXERLINE>();

            List<IntPtr> listMixerHandle = new List<IntPtr>();
            MixerCaps mixCap;
            MIXERLINE mixLine;
            IntPtr mixerHandle = IntPtr.Zero;
            //Get Mixer numbers
            var rev = PCWin32.mixerGetNumDevs();
            for (int mixerId = 0; mixerId < rev; mixerId++)
            {
                mixCap = new MixerCaps();
                PCWin32.MixerGetDevCaps(mixerId, ref mixCap, Marshal.SizeOf(mixCap));
                lstMixerCaps.Add(mixCap);
                PCWin32.MixerOpen(ref mixerHandle, (uint)mixerId, IntPtr.Zero, IntPtr.Zero, 0);
                listMixerHandle.Add(mixerHandle);
            }
            int mixerID = 0;
            //This will return the ID as same as list index
            PCWin32.MixerGetID(listMixerHandle[1], out mixerID, 0);

            for (int i = 0; i < listMixerHandle.Count; i++)
            {
                mixLine = new MIXERLINE();
                //Get Speaker
                mixLine.StructSize = (uint)Marshal.SizeOf(mixLine);

                switch (i)
                {
                    case 0:
                        mixLine.ComponentType = VolumeConstants.MIXERLINE_COMPONENTTYPE.DST_SPEAKERS;
                        break;
                    case 1:
                        mixLine.ComponentType = VolumeConstants.MIXERLINE_COMPONENTTYPE.DST_SPEAKERS;
                        break;
                    case 2:
                        mixLine.ComponentType = VolumeConstants.MIXERLINE_COMPONENTTYPE.SRC_MICROPHONE;
                        break;
                }
                rev = PCWin32.MixerGetLineInfo(listMixerHandle[i], ref mixLine, (uint)MixerLineInfoType.MIXER_GETLINEINFOF_COMPONENTTYPE);
                lstMixerLine.Add(mixLine);
            }
            foreach (var mixerHand in listMixerHandle)
            {
                PCWin32.MixerClose(mixerHand);
            }
        }
    }
}
