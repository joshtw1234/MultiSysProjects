using System;
using System.Collections.ObjectModel;
using System.Windows;
using UtilityUILib;
using WPFLogicDemo.Models;

namespace WPFLogicDemo.ViewModels
{
    class WPFLogicControlViewModel : BindAbleBases
    {
        IWPFLogicModel _wpflogicModel;

        ResourceDictionary _resDictionary;

        public WPFLogicControlViewModel(IWPFLogicModel wpflogicModel)
        {
            _wpflogicModel = wpflogicModel;
            _resDictionary = _wpflogicModel.GetLocalStyle();
            _commonButtonCollection = GetCommonButtons();
        }

        private ObservableCollection<IMenuItem> GetCommonButtons()
        {
            return new ObservableCollection<IMenuItem>()
            {
                new MenuItem()
                {
                    MenuName = "Clear",
                    MenuStyle = _resDictionary["LogicBtnStyle"] as Style
                }
            };
        }

        private ObservableCollection<IMenuItem> _commonButtonCollection;
        public ObservableCollection<IMenuItem> CommonButtonCollection
        {
            get
            {
                return _commonButtonCollection;
            }

            set
            {
                onPropertyChanged(this, "CommonButtonCollection");
            }
        }
    }
}
