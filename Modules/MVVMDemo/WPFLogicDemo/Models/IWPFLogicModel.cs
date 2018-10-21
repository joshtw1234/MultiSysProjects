using System.Collections.ObjectModel;
using UtilityUILib;

namespace WPFLogicDemo.Models
{
    /// <summary>
    /// The Interface for VM get GUI bind properties.
    /// </summary>
    interface IWPFLogicModel
    {
        ObservableCollection<IMenuItem> GetPageItemsSource();
        LogicContentData GetLogicContentVM();
        STOCKContentData GetSTOCKContentVM();
    }
}
