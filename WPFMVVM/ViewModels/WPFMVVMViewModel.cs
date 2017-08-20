using System.ComponentModel;

namespace WPFMVVM.ViewModels
{
    public class WPFMVVMViewModel : INotifyPropertyChanged
    {
        public WPFMVVMViewModel(WPFMVVM.Models.WPFMVVMModel _model)
        {

        }

        #region INotifyPropertyChanged Interface
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            { PropertyChanged(sender, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion

        string btnName = "Shit";
        public string BtnName { get { return btnName; } set { btnName = value; onPropertyChanged(this, "BtnName"); } }
    }
}
