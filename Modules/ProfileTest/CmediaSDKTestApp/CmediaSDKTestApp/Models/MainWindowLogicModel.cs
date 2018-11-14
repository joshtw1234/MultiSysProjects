using CmediaSDKTestApp.BaseModels;
using MYCmediaSDK.MYSDK.Structures;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CmediaSDKTestApp.Models
{
    class MainWindowLogicModel
    {
        protected MicPageContentModel _micPage;
        protected ObservableCollection<IMenuItem> _contentpages;
        private MyDelegateCommond<string> _onCommonButtonClickEvent;
        protected MyDelegateCommond<string> OnCommonButtonClickEvent => _onCommonButtonClickEvent ?? (_onCommonButtonClickEvent = new MyDelegateCommond<string>(OnCommonButtonClick));
        private MyDelegateCommond<string> _onPageButtonClickEvent;
        protected MyDelegateCommond<string> OnPageButtonClickEvent => _onPageButtonClickEvent ?? (_onPageButtonClickEvent = new MyDelegateCommond<string>(OnPageButtonClick));
        private MyDelegateCommond<RoutedPropertyChangedEventArgs<double>> _onSliderValueChangeCommand;
        protected MyDelegateCommond<RoutedPropertyChangedEventArgs<double>> OnSliderValueChangeCommand => _onSliderValueChangeCommand ?? (_onSliderValueChangeCommand = new MyDelegateCommond<RoutedPropertyChangedEventArgs<double>>(OnSliderValueChanged));
        private MyDelegateCommond<RoutedEventArgs> _onMuteBoxCheckedCommand;
        protected MyDelegateCommond<RoutedEventArgs> OnMuteBoxCheckedCommand => _onMuteBoxCheckedCommand ?? (_onMuteBoxCheckedCommand = new MyDelegateCommond<RoutedEventArgs>(OnMuteBoxChecked));

        protected virtual void OnMuteBoxChecked(RoutedEventArgs obj)
        {
            //Base checkbox event
        }

        protected virtual void OnSliderValueChanged(RoutedPropertyChangedEventArgs<double> obj)
        {
            //base slider value changed event
        }

        protected virtual void OnPageButtonClick(string obj)
        {
            var btn = (ButtonStrings)Enum.Parse(typeof(ButtonStrings), obj);
            foreach (IMenuItem page in _contentpages)
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
            switch (btn)
            {
                case ButtonStrings.Apply:
                    break;
                case ButtonStrings.Cancle:
                    _micPage.DisplayText.MenuName = string.Empty;
                    break;
                case ButtonStrings.EnableMagic:
                    //InitialCaptureDevice(_cmediajackInfoCapture, CmediaCaptureFunctionPoint.Enable_MAGICVOICE);
                    break;
                case ButtonStrings.EnableEcho:
                    //InitialCaptureDevice(_cmediajackInfoCapture, CmediaCaptureFunctionPoint.Enable_MICECHO);
                    break;
            }
        }

        protected VolumeControlStructure _audioVolumeControl = null;
        protected VolumeControlStructure _microphoneVolumeControl = null;
    }

    enum ButtonStrings
    {
        Equaliser,
        Microphone,
        Lighting,
        Cooling,
        Cancle,
        Apply,
        EnableMagic,
        EnableEcho
    }
}
