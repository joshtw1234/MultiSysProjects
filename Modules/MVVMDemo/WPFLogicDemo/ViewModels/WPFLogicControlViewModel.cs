using System.Collections.ObjectModel;
using UtilityUILib;
using WPFLogicDemo.Models;

namespace WPFLogicDemo.ViewModels
{
    class WPFLogicControlViewModel
    {
        IWPFLogicModel _wpflogicModel;

        public WPFLogicControlViewModel(IWPFLogicModel wpflogicModel)
        {
            _wpflogicModel = wpflogicModel;
            CommonButtonCollection = (_wpflogicModel as WPFLogicUIModel).GetCommonButtons();
            MessageText = (_wpflogicModel as WPFLogicUIModel).GetMessageText();
        }

        public ObservableCollection<IMenuItem> CommonButtonCollection { get; set; }

        public IMenuItem MessageText { get; set; }
    }
}
