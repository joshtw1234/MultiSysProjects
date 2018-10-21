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
            PageItemsSource = _wpflogicModel.GetPageItemsSource();
            LogicContentVM = _wpflogicModel.GetLogicContentVM();
            STOCKContentVM = _wpflogicModel.GetSTOCKContentVM();
        }

        public ObservableCollection<IMenuItem> PageItemsSource { get; set; }

        public LogicContentData LogicContentVM { get; set; }

        public STOCKContentData STOCKContentVM { get; set; }

    }

    
}
