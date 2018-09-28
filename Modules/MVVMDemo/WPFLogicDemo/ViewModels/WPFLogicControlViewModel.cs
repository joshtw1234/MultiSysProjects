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
            CommonButtonCollection = _wpflogicModel.GetCommonButtons();
            MessageText = _wpflogicModel.GetMessageText();
        }

        public ObservableCollection<IMenuItem> CommonButtonCollection { get; set; }

        public IMenuItem MessageText { get; set; }
    }
}
