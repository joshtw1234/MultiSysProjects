using AudioDemoModule.Enums;
using CommonUILib.Models;
using CommonUILib.Structures;
using Prism.Commands;
using System;
using System.Windows;

namespace AudioDemoModule.ViewModels
{
    class AudioControlViewModel : BaseViewModel
    {
        public TextBoxTip AudioTitle { get; set; }
        public TextBoxTip MonitorTitle { get; set; }

        private DelegateCommand<string> _onTipButtonClickEvent;
        public DelegateCommand<string> OnTipButtonClickEvent
        {
            get
            {
                return _onTipButtonClickEvent ?? (_onTipButtonClickEvent = new DelegateCommand<string>(OnTipButtonClick));
            }
        }

        private DelegateCommand<string> _onTipCloseClickEvent;
        public DelegateCommand<string> OnTipCloseClickEvent
        {
            get
            {
                return _onTipCloseClickEvent ?? (_onTipCloseClickEvent = new DelegateCommand<string>(OnTipCloseClick));
            }
        }

        

        public AudioControlViewModel()
        {
            //var tt = Application.Current.Resources["InfoButton"] as Style;
            AudioTitle = new TextBoxTip()
            {
                TitleText = new TipMessageItem()
                {
                    MenuName = AudioControlStrings.Volume.ToString(),
                    MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style
                },
                TipItem = new TipViewItem()
                {
                    TipViewItemVisibility = true,
                    TipButton = new TipMessageItem()
                    {
                        MenuName = "I",
                        MenuStyle = Application.Current.Resources["InfoButton"] as Style,
                        MenuCommand = OnTipButtonClickEvent,
                        MenuData = AudioControlStrings.Volume.ToString()
                    },
                    TipInfo = new TipMessageItem()
                    {
                        MenuName = "Tip String is here",
                        MenuStyle = Application.Current.Resources["ToolTipStyle"] as Style,
                        MenuCommand = OnTipCloseClickEvent,
                        MenuData = AudioControlStrings.Volume.ToString()
                    }
                }
            };

            MonitorTitle = new TextBoxTip()
            {
                TitleText = new TipMessageItem()
                {
                    MenuName = "Monitor Audio",
                    MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style
                },
                TipItem = new TipViewItem()
                {
                    TipViewItemVisibility = true,
                    TipButton = new TipMessageItem()
                    {
                        MenuName = "I",
                        MenuStyle = Application.Current.Resources["InfoButton"] as Style,
                        MenuCommand = OnTipButtonClickEvent,
                        MenuData = AudioControlStrings.Monitor_Speaker.ToString()
                    },
                    TipInfo = new TipMessageItem()
                    {
                        MenuName = "Tip String is here",
                        MenuStyle = Application.Current.Resources["ToolTipStyle"] as Style,
                        MenuCommand = OnTipCloseClickEvent,
                        MenuData = AudioControlStrings.Monitor_Speaker.ToString()
                    }
                }
            };
        }

        private void ConfigureTipVisibility(string obj)
        {
            var btnData = (AudioControlStrings)Enum.Parse(typeof(AudioControlStrings), obj);
            switch (btnData)
            {
                case AudioControlStrings.Volume:
                    AudioTitle.TipItem.TipInfo.MenuVisibility = !AudioTitle.TipItem.TipInfo.MenuVisibility;
                    break;
                case AudioControlStrings.Monitor_Speaker:
                    MonitorTitle.TipItem.TipInfo.MenuVisibility = !MonitorTitle.TipItem.TipInfo.MenuVisibility;
                    break;
                default:
                    //TODO: Implement AudioControlsCollection
                    //var data = AudioControlsCollection.FirstOrDefault(x => x.ControlTitle.TipItem.TipButton.MenuData.Equals(obj));
                    //data.ControlTitle.TipItem.TipInfo.MenuVisibility = !data.ControlTitle.TipItem.TipInfo.MenuVisibility;
                    break;
            }
        }

        private void OnTipButtonClick(string obj)
        {
            ConfigureTipVisibility(obj);
        }

        private void OnTipCloseClick(string obj)
        {
            ConfigureTipVisibility(obj);
        }

    }
}
