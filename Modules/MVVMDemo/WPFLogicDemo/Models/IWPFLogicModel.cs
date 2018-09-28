using System.Collections.ObjectModel;
using UtilityUILib;

namespace WPFLogicDemo.Models
{
    interface IWPFLogicModel
    {
        ObservableCollection<IMenuItem> GetCommonButtons();
        IMenuItem GetMessageText();
    }
}
