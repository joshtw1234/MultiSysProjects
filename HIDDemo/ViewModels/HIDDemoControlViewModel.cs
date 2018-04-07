using HIDDemo.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using UtilityUILib;

namespace HIDDemo.ViewModels
{
    public class HIDDemoControlViewModel : INotifyPropertyChanged
    {
        private IHIDDemoControlModel hidGUIModel;

        #region INotifyPropertyChanged Interface
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            { PropertyChanged(sender, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion

        private ObservableCollection<IMenuItem> hidOPButtonCollection;

        public ObservableCollection<IMenuItem> HIDOPButtonCollection
        {
            get
            {
                return hidOPButtonCollection;
            }

            set
            {
                hidOPButtonCollection = value;
                onPropertyChanged(this, "HIDOPButtonCollection");
            }
        }

        private ObservableCollection<IMenuItem> hidDisplayCollections;

        public ObservableCollection<IMenuItem> HIDDisplayCollections
        {
            get
            {
                return hidDisplayCollections;
            }

            set
            {
                hidDisplayCollections = value;
                onPropertyChanged(this, "HIDDisplayCollections");
            }
        }

        public HIDDemoControlViewModel(IHIDDemoControlModel _model)
        {
            hidGUIModel = _model;
            hidOPButtonCollection = hidGUIModel.GetHIDOPButtons;
            hidDisplayCollections = GetHIDDisplayItems;
        }

        public ObservableCollection<IMenuItem> GetHIDDisplayItems
        {
            get
            {
                //hidGUIModel.GetHIDInfoCollections;
                return new ObservableCollection<IMenuItem>()
                {
                    new HIDDisplayItem()
                    {
                        DisplayType = HIDDisplayItemEnum.Lebel,
                        MenuName = "PID",
                        //MenuStyle = (Style)Application.Current.FindResource("DisplayLabelStyle")
                    },
                    new HIDDisplayItem()
                    {
                        DisplayType = HIDDisplayItemEnum.TextBlock,
                        MenuName = "PID"
                    },
                };
            }
        }
    }
}
