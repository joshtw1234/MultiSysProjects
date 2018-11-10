using CmediaSDKTestApp.BaseModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CmediaSDKTestApp.Models
{
    class MainWindowLogicModel
    {
        protected MicPageContentModel _micPage;
        protected ObservableCollection<IMenuItem> _contentpages;
        private MyCommond<string> _onCommonButtonClickEvent;
        protected MyCommond<string> OnCommonButtonClickEvent => _onCommonButtonClickEvent ?? (_onCommonButtonClickEvent = new MyCommond<string>(OnCommonButtonClick));
        private MyCommond<string> _onPageButtonClickEvent;
        protected MyCommond<string> OnPageButtonClickEvent => _onPageButtonClickEvent ?? (_onPageButtonClickEvent = new MyCommond<string>(OnPageButtonClick));

        private void OnPageButtonClick(string obj)
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

        protected OMENVolumeControlStructure _audioVolumeControl = null;
        protected OMENVolumeControlStructure _microphoneVolumeControl = null;
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
