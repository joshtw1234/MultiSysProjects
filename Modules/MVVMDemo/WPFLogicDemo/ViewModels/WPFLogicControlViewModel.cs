using System.Collections.ObjectModel;
using UtilityUILib;
using WPFLogicDemo.Models;

namespace WPFLogicDemo.ViewModels
{
    /// <summary>
    /// ViewModel no need Bindable class.
    /// Just Contain properties
    /// TODO: enhance to use interface for reusable.
    /// </summary>
    class WPFLogicControlViewModel
    {
        IWPFLogicModel _wpflogicModel;

        public WPFLogicControlViewModel(IWPFLogicModel wpflogicModel)
        {
            _wpflogicModel = wpflogicModel;
            CommonButtonCollection = _wpflogicModel.GetCommonButtons();
            MessageText = _wpflogicModel.GetMessageText();
            TextInput = _wpflogicModel.GetTextInput();
        }

        public ObservableCollection<IMenuItem> CommonButtonCollection { get; set; }

        public IMenuItem MessageText { get; set; }

        public IMenuItem TextInput { get; set; }
    }
}
