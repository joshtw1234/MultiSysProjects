using System.Collections.ObjectModel;
using System.Windows;
using UtilityUILib;

namespace WPFLogicDemo.Models
{
    class LogicContentData : BaseContentData
    {
        public ObservableCollection<IMenuItem> CommonButtonCollection { get; set; }

        public IMenuItem MessageText { get; set; }

        public IMenuItem TextInput { get; set; }
    }
}
