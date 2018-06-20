using HIDHeadSet.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using UtilityUILib;

namespace HIDHeadSet.ViewModels
{
    class HeadSetControlViewModel : BindAbleBases
    {
        private IHeadSetModel headSetModel;
        private ResourceDictionary headSetResource;
        public HeadSetControlViewModel(IHeadSetModel _model)
        {
            headSetModel = _model;
            headSetResource = new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/HIDHeadSet;component/Styles/HeadSetStyle.xaml", UriKind.RelativeOrAbsolute)
            };

            mainItems = GetMainItems();
        }

        private ObservableCollection<MainItemDC> GetMainItems()
        {
            return new ObservableCollection<MainItemDC>()
            {
                new MainItemDC()
                {
                    TitleString = new MenuItem()
                    {
                        MenuName = "Hello Word!!22",
                        MenuStyle = headSetResource["StyleTitle"] as Style
                    }
                }
            };
        }

        private ObservableCollection<MainItemDC> mainItems;

        public ObservableCollection<MainItemDC> MainItems
        {
            get
            {
                return mainItems;
            }
            set
            {
                mainItems = value;
                onPropertyChanged(this, "HIDOPButtonCollection");
            }
        }

    }

    class MainItemDC
    {
        public IMenuItem TitleString { get; set; }
        public ObservableCollection<IMenuItem> SubItems { get; set; }
    }
}
