using System.Windows;
using UtilityUILib;

namespace WPFLogicDemo.Models
{
    class BaseContentData : BindAbleBases
    {
        private Visibility _contentVisibility;
        public Visibility ContentVisibility
        {
            get
            {
                return _contentVisibility;
            }
            set
            {
                _contentVisibility = value;
                onPropertyChanged(this, "ContentVisibility");
            }
        }
    }
}
