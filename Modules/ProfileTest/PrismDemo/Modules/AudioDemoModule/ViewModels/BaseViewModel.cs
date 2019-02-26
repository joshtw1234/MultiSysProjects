using Prism.Mvvm;
using System.ComponentModel;

namespace AudioDemoModule.ViewModels
{
    abstract class BaseViewModel : BindableBase
    {
        public string ViewModelName { get; set; }

        private bool _isControlVisible;
    
        public bool IsControlVisible
        {
            get
            {
                return _isControlVisible;
            }
            set
            {
                SetProperty(ref _isControlVisible, value);
            }
        }
    }
}
