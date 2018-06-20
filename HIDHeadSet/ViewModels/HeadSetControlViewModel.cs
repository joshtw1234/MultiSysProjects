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
                    TitleStrings = new ObservableCollection<IMenuItem>()
                    {
                        new MenuItem()
                        {
                        MenuName = "LED Control",
                        MenuStyle = headSetResource["StyleTitle"] as Style
                        },
                        new MenuItem()
                        {
                        MenuName = "Operation",
                        MenuStyle = headSetResource["StyleSubTitle"] as Style
                        }
                    },
                    SubItems = new ObservableCollection<IMenuItem>()
                    {
                        new MenuItem()
                        {
                            MenuName = "Static",
                            MenuStyle = headSetResource["StyleCheckBox"] as Style
                        }
                    }
                },
                new MainItemDC()
                {
                    TitleStrings = new ObservableCollection<IMenuItem>()
                    {
                        new MenuItem()
                        {
                        MenuName = "Fan Control",
                        MenuStyle = headSetResource["StyleTitle"] as Style
                        },
                        new MenuItem()
                        {
                        MenuName = "Cooling",
                        MenuStyle = headSetResource["StyleSubTitle"] as Style
                        }
                    },
                    SubItems = new ObservableCollection<IMenuItem>()
                    {
                        new MenuItem()
                        {
                            MenuName = "Off",
                            MenuStyle = headSetResource["StyleCheckBox"] as Style
                        }
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
        public ObservableCollection<IMenuItem> TitleStrings { get; set; }
        public ObservableCollection<IMenuItem> SubItems { get; set; }
    }
}
