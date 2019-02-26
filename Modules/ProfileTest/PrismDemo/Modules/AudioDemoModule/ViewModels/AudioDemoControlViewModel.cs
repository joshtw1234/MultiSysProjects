using AudioDemoModule.Enums;
using AudioDemoModule.Interfaces;
using CommonUILib.Interfaces;
using CommonUILib.Models;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AudioDemoModule.ViewModels
{
    class AudioDemoControlViewModel
    {
        IAudioDemoControlModel _model;

        private ResourceDictionary _localDictionary;
        public ObservableCollection<IViewItem> PageButtons { get; set; }
        public ObservableCollection<IViewItem> CommonButtons { get; set; }

        public BaseViewModel AudioControlsDataContext { get; set; }
        public BaseViewModel AdvanceControlDataContext { get; set; }

        private DelegateCommand<string> _onPageButtonClickEvent;
        private DelegateCommand<string> OnPageButtonClickEvent => _onPageButtonClickEvent ?? (_onPageButtonClickEvent = new DelegateCommand<string>(OnPageButtonClick));

        private List<BaseViewModel> _listViewModels;

        public AudioDemoControlViewModel(IAudioDemoControlModel model)
        {
            _model = model;
            PageButtons = GetPageButtons();

            AudioControlsDataContext = new AudioControlViewModel() { ViewModelName = SubControls.Audio.ToString() };
            AdvanceControlDataContext = new AdvanceControlViewModel() { ViewModelName = SubControls.Advance.ToString() };

            _listViewModels = new List<BaseViewModel>()
            {
                AudioControlsDataContext,
                AdvanceControlDataContext
            };
        }

        private ObservableCollection<IViewItem> GetPageButtons()
        {
            return new ObservableCollection<IViewItem>()
            {
                new ViewItem()
                {
                    MenuName = "Audio Control",
                    MenuStyle = Application.Current.Resources["BaseToggleButtonStyle"] as Style,
                    MenuCommand = OnPageButtonClickEvent,
                    MenuData = SubControls.Audio.ToString()
                },
                new ViewItem()
                {
                    MenuName = "Advance Control",
                    MenuStyle = Application.Current.Resources["BaseToggleButtonStyle"] as Style,
                    MenuCommand = OnPageButtonClickEvent,
                    MenuData = SubControls.Advance.ToString()
                },
            };
        }

        private void OnPageButtonClick(string obj)
        {
            SetAllViewModelsDisable();
            var displayVM = _listViewModels.FirstOrDefault(x => x.ViewModelName != null && x.ViewModelName.Equals(obj));
            if (displayVM != null) displayVM.IsControlVisible = true;
        }

        private void SetAllViewModelsDisable()
        {
            foreach (BaseViewModel baseVM in _listViewModels)
            {
                baseVM.IsControlVisible = false;
            }
        }
    }
}
