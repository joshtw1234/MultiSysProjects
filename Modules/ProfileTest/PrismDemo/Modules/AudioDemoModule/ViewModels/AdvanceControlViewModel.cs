using AudioDemoModule.Enums;
using AudioDemoModule.Interfaces;
using AudioDemoModule.Structures;
using CommonUILib.Interfaces;
using CommonUILib.Models;
using CommonUILib.Structures;
using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AudioDemoModule.ViewModels
{
    class AdvanceControlViewModel : BaseViewModel
    {
        private IAudioDemoControlModel _model;

        const string SliderMax = "20.0";
        const string SliderMin = "-20.0";
        const string SliderStep = "1.0";
        private ResourceDictionary _localResourceDictionary;
        Dictionary<EqualizerPresets, List<int>> _presetDictionary;
        #region Property
        public TextBoxTip AudioTitle { get; set; }
        public TextBoxTip CustomEQPreset { get; set; }
        private DelegateCommand<string> _onAudioModeClickEvent;
        private DelegateCommand<string> OnAudioModeClickEvent => _onAudioModeClickEvent ?? (_onAudioModeClickEvent = new DelegateCommand<string>(OnEQModeClick));
        private DelegateCommand<string> _onBandsRadioButtonCheckedEvent;
        private DelegateCommand<string> OnBandsRadioButtonCheckedEvent => _onBandsRadioButtonCheckedEvent ?? (_onBandsRadioButtonCheckedEvent = new DelegateCommand<string>(OnBandsRadioButtonChecked));


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
        private DelegateCommand<RoutedPropertyChangedEventArgs<double>> _onEQBandValueChangedEvent;
        private DelegateCommand<RoutedPropertyChangedEventArgs<double>> OnEQBandValueChangedEvent => _onEQBandValueChangedEvent ?? (_onEQBandValueChangedEvent = new DelegateCommand<RoutedPropertyChangedEventArgs<double>>(OnEQBandValueChanged));
        public ObservableCollection<IViewItem> EQModeCollection { get; set; }
        public ObservableCollection<IViewItem> EQPresetModeCollection { get; set; }
        public ObservableCollection<IViewItem> ControlButtonCollection { get; set; }
        public ObservableCollection<EQBandItem> EQBandCollection { get; set; }
        public EQBandItem BandClarity { get; set; }
        public ObservableCollection<IViewItem> EQBandsCollection { get; set; }
        public ObservableCollection<IViewItem> PresetControlCollection { get; set; }
        #endregion

        public AdvanceControlViewModel(IAudioDemoControlModel model)
        {
            _model = model;
            _localResourceDictionary = new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/AudioDemoModule;component/Styles/AudioDemoStyle.xaml", UriKind.RelativeOrAbsolute)
            };

            //Force EQ enabled
            //if (!_model.GetEQEnabled())
            //{
            //    _model.SetEQEnabled();
            //}
            //Get Cmedia EQ presets
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AudioDemoModule.JSON.EqualizerPreset.json"))
            {
                System.IO.TextReader tr = new System.IO.StreamReader(stream);
                string fileContents = tr.ReadToEnd();
                _presetDictionary = JsonConvert.DeserializeObject<Dictionary<EqualizerPresets, List<int>>>(fileContents);
            }

            AudioTitle = new TextBoxTip()
            {
                TitleText = new ViewItem()
                {
                    MenuName = "Audio Mode",
                    MenuStyle = _localResourceDictionary["BaseTextBoxStyle"] as Style
                },
                TipItem = new TipViewItem()
                {
                    TipViewItemVisibility = true,
                    TipButton = new ViewItem()
                    {
                        MenuName = "I",
                        MenuStyle = _localResourceDictionary["InfoButton"] as Style,
                        MenuCommand = OnTipButtonClickEvent,
                        MenuData = EQModesStrings.Audio_Modes.ToString()
                    },
                    TipInfo = new ViewItem()
                    {
                        MenuName = "Tip String is here",
                        MenuStyle = _localResourceDictionary["ToolTipStyle"] as Style,
                        MenuCommand = OnTipCloseClickEvent,
                        MenuData = EQModesStrings.Audio_Modes.ToString()
                    }
                }
            };

            CustomEQPreset = new TextBoxTip()
            {
                TitleText = new ViewItem()
                {
                    MenuName = "Custom EQ Presets",
                    MenuStyle = _localResourceDictionary["BaseTextBoxStyle"] as Style
                },
                TipItem = new TipViewItem()
                {
                    TipViewItemVisibility = true,
                    TipButton = new ViewItem()
                    {
                        MenuName = "I",
                        MenuStyle = _localResourceDictionary["InfoButton"] as Style,
                        MenuCommand = OnTipButtonClickEvent,
                        MenuData = EQModesStrings.Custom_EQ_Presets.ToString()
                    },
                    TipInfo = new ViewItem()
                    {
                        MenuName = "Custom EQ Tip String is here",
                        MenuStyle = _localResourceDictionary["ToolTipStyle"] as Style,
                        MenuCommand = OnTipCloseClickEvent,
                        MenuData = EQModesStrings.Custom_EQ_Presets.ToString()
                    }
                }
            };

            EQModeCollection = GetEQModeCollection();
            EQPresetModeCollection = GetEQPresetModeCollection();
            EQBandCollection = GetEQBandeCollection();
            //SetFiveBands();
            BandClarity = new EQBandItem()
            {
                EQBandSlider = new AudioSliderItem()
                {
                    MenuName = AudioControlStrings.Headphone.ToString(),
                    MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                    MaxValue = SliderMax,
                    MinValue = SliderMin,
                    Step = SliderStep,
                    SliderValue = "0",
                    SliderValueChanged = OnEQBandValueChangedEvent
                },
                EQBandName = new ViewItem() { MenuName = "Dialog Clarity", MenuStyle = _localResourceDictionary["VerticalBandStyle"] as Style, MenuVisibility = true }
            };
            EQBandsCollection = GetEQBandsCollection();
            PresetControlCollection = GetPreSetControlCollection();
            ControlButtonCollection = new ObservableCollection<IViewItem>()
            {
                new ViewItem()
                {
                    MenuName = "Restore Default",
                    MenuStyle = _localResourceDictionary["BaseOMENButtonStyle"] as Style,
                    MenuData = "Restore Default"
                }
            };
            //_model.RegisterSpeakerFeatrueCallBack(OnSpeakerFeatureChanged);
        }

        private ObservableCollection<IViewItem> GetEQPresetModeCollection()
        {
            var returnCollection = new ObservableCollection<IViewItem>();
            foreach (var item in _presetDictionary)
            {
                var presetMode = new ViewItem()
                {
                    MenuName = item.Key.ToString(),
                    MenuStyle = _localResourceDictionary["BaseOMENButtonStyle"] as Style,
                    MenuCommand = OnAudioModeClickEvent,
                    MenuData = item.Key.ToString()
                };
                returnCollection.Add(presetMode);
            }
            return returnCollection;
        }

        private void OnSpeakerFeatureChanged()
        {
            foreach (var band in EQBandCollection)
            {
                var slider = band.EQBandSlider as AudioSliderItem;
                var bandNum = (Bands)Enum.Parse(typeof(Bands), slider.MenuName);
                //slider.SliderValue = _model.GetEQBandValue(bandNum);
            }
        }

        private ObservableCollection<IViewItem> GetPreSetControlCollection()
        {
            return new ObservableCollection<IViewItem>()
            {
                new ViewItem()
                {
                    MenuName = "Add New",
                    MenuStyle = _localResourceDictionary["PresetControlButtonStyle"] as Style

                },
                new ViewItem()
                {
                    MenuName = "Delete",
                    MenuStyle = _localResourceDictionary["PresetControlButtonStyle"] as Style
                }
            };
        }

        private ObservableCollection<IViewItem> GetEQBandsCollection()
        {
            return new ObservableCollection<IViewItem>()
            {
                new ViewItem()
                {
                    MenuName = "10 Bands EQ",
                    MenuStyle = Application.Current.Resources["BaseRadioButtonStyle"] as Style,
                    MenuCommand = OnBandsRadioButtonCheckedEvent,
                    MenuData = "10",
                    MenuChecked = true
                },
                new ViewItem()
                {
                    MenuName = "5 Bands EQ",
                    MenuStyle = Application.Current.Resources["BaseRadioButtonStyle"] as Style,
                    MenuCommand = OnBandsRadioButtonCheckedEvent,
                    MenuData = "5",
                }
            };
        }

        private void OnBandsRadioButtonChecked(string obj)
        {
            for (Bands i = Bands.Band1; i <= Bands.Band10; i++)
            {
                EQBandCollection[(int)i].EQBandName.MenuVisibility = true;
                EQBandCollection[(int)i].EQBandName.MenuName = i.ToString();
            }
            if (obj.Equals("5"))
            {
                SetFiveBands();
            }
        }

        private void SetFiveBands()
        {
            for (int i = 0; i < EQBandCollection.Count; i += 2)
            {
                EQBandCollection[i].EQBandName.MenuVisibility = false;
            }
            var visBand = EQBandCollection.Where(x => x.EQBandName.MenuVisibility == true).ToList();
            visBand[0].EQBandName.MenuName = string.Empty;
            visBand[1].EQBandName.MenuName = BandName.Bass.ToString();
            visBand[2].EQBandName.MenuName = string.Empty;
            visBand[3].EQBandName.MenuName = BandName.Mid.ToString();
            visBand[4].EQBandName.MenuName = BandName.Treble.ToString();
        }

        private void OnEQModeClick(string obj)
        {
            try
            {
                var modeName = (EqualizerPresets)Enum.Parse(typeof(EqualizerPresets), obj);
                for (int i = 0; i < EQBandCollection.Count; i++)
                {
                    //(EQBandCollection[i].EQBandSlider as AudioSliderItem).SliderValue = _presetDictionary[modeName][i].ToString();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void OnEQBandValueChanged(RoutedPropertyChangedEventArgs<double> obj)
        {
            var bandsMode = EQBandsCollection.FirstOrDefault(x => x.MenuChecked);
            var band = (obj.Source as FrameworkElement).DataContext as AudioSliderItem;
            var bandNum = (Bands)Enum.Parse(typeof(Bands), band.MenuName);
            if (bandsMode.MenuData.Equals("5"))
            {
                //TODO Set 5 band to 10 band
            }
            else
            {
                //_model.SetBandValue(bandNum, obj.NewValue);
            }
        }

        private ObservableCollection<IViewItem> GetEQModeCollection()
        {
            return new ObservableCollection<IViewItem>()
            {
                new ViewItem()
                {
                    MenuName = "GAMING",
                    MenuStyle = _localResourceDictionary["BaseOMENButtonStyle"] as Style,
                    MenuCommand = OnAudioModeClickEvent,
                    MenuData = PresetName.GAMING.ToString()
                },
                new ViewItem()
                {
                    MenuName = "CINEMA",
                    MenuStyle = _localResourceDictionary["BaseOMENButtonStyle"] as Style,
                    MenuCommand = OnAudioModeClickEvent,
                    MenuData = PresetName.CINEMA.ToString()
                },
                new ViewItem()
                {
                    MenuName = "MUSIC",
                    MenuStyle = _localResourceDictionary["BaseOMENButtonStyle"] as Style,
                    MenuCommand = OnAudioModeClickEvent,
                    MenuData = PresetName.MUSIC.ToString()
                },
                new ViewItem()
                {
                    MenuName = "VOICE",
                    MenuStyle = _localResourceDictionary["BaseOMENButtonStyle"] as Style,
                    MenuCommand = OnAudioModeClickEvent,
                    MenuData = PresetName.VOICE.ToString()
                },
                new ViewItem()
                {
                    MenuName = "CUSTOM",
                    MenuStyle = _localResourceDictionary["BaseOMENButtonStyle"] as Style,
                    MenuCommand = OnAudioModeClickEvent,
                    MenuData = PresetName.CUSTOM.ToString()
                }
            };
        }

        private ObservableCollection<EQBandItem> GetEQBandeCollection()
        {
#if true
            var rev = new ObservableCollection<EQBandItem>();
            for (Bands i = Bands.Band1; i <= Bands.Band10; i++)
            {
                var bandItem = new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = i.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        //SliderValue = _model.GetEQBandValue(i),
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },
                    EQBandName = new EQBandNameItem() { MenuName = i.ToString(), MenuVisibility = true }
                };
                rev.Add(bandItem);
            }
            return rev;
#else
            return new ObservableCollection<EQBandItem>()
            {
                new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = Bands.Band1.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        SliderValue = "50",
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },

                },
                new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = Bands.Band2.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        SliderValue = "50",
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },
                    EQBandName = new ViewItem() { MenuName= BandName.Bass.ToString() }
                },
                new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = Bands.Band3.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        SliderValue = "50",
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },

                },
                new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = Bands.Band4.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        SliderValue = "50",
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },
                    EQBandName = new ViewItem() { MenuName= Bands.Band4.ToString() }
                },
                new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = Bands.Band5.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        SliderValue = "50",
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },
                    EQBandName = new ViewItem()
                    {
                        MenuName = Bands.Band5.ToString()
                    }
                },
                new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = Bands.Band4.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        SliderValue = "50",
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },
                    EQBandName = new ViewItem() { MenuName= Bands.Band4.ToString() }
                },
                new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = Bands.Band4.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        SliderValue = "50",
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },
                    EQBandName = new ViewItem() { MenuName= Bands.Band4.ToString() }
                },
                new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = Bands.Band4.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        SliderValue = "50",
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },
                    EQBandName = new ViewItem() { MenuName= Bands.Band4.ToString() }
                },
                new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = Bands.Band9.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        SliderValue = "50",
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },
                    EQBandName = new ViewItem() { MenuName= Bands.Band9.ToString() }
                },
                new EQBandItem()
                {
                    EQBandSlider = new AudioSliderItem()
                    {
                        MenuName = Bands.Band10.ToString(),
                        MenuStyle = _localResourceDictionary["CustomAudioSlider"] as Style,
                        MaxValue = SliderMax,
                        MinValue = SliderMin,
                        Step = SliderStep,
                        SliderValue = "50",
                        SliderValueChanged = OnEQBandValueChangedEvent
                    },
                    EQBandName = new ViewItem() { MenuName= Bands.Band10.ToString() }
                }
            };
#endif
        }

        private void ConfigureTipVisibility(string obj)
        {
            var btnData = (EQModesStrings)Enum.Parse(typeof(EQModesStrings), obj);
            switch (btnData)
            {
                case EQModesStrings.Audio_Modes:
                    AudioTitle.TipItem.TipInfo.MenuVisibility = !AudioTitle.TipItem.TipInfo.MenuVisibility;
                    break;
                case EQModesStrings.Custom_EQ_Presets:
                    CustomEQPreset.TipItem.TipInfo.MenuVisibility = !CustomEQPreset.TipItem.TipInfo.MenuVisibility;
                    break;
                default:
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
