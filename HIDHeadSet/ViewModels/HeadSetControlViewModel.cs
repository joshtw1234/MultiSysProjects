using HIDHeadSet.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using UtilityUILib;

namespace HIDHeadSet.ViewModels
{
    class HeadSetControlViewModel : BindAbleBases
    {
        const string OpenCmd= "Open";
        const string CloseCmd = "Close";
        const string SendCmd = "Send";

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
            mainButtons = GetMainButtons();
            if(!headSetModel.Initialize())
            {
                //Not found HID
                foreach(var btn in mainButtons)
                {
                    btn.MenuEnabled = false;
                }
            }
        }

        #region Properties
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

        private ObservableCollection<IMenuItem> mainButtons;
        public ObservableCollection<IMenuItem> MainButtons
        {
            get
            {
                return mainButtons;
            }
            set
            {
                mainButtons = value;
                onPropertyChanged(this, "MainButtons");
            }
        }
        #endregion

        private ObservableCollection<IMenuItem> GetMainButtons()
        {
            return new ObservableCollection<IMenuItem>()
            {
                new MenuItem()
                {
                    MenuName = OpenCmd,
                    MenuData = OpenCmd,
                    MenuStyle = headSetResource["StyleMainButton"] as Style,
                    MenuEnabled = true,
                    MenuCommand = new MyCommond<string>(OnButtonClick)
                },
                new MenuItem()
                {
                    MenuName = CloseCmd,
                    MenuData = CloseCmd,
                    MenuStyle = headSetResource["StyleMainButton"] as Style,
                    MenuEnabled = true,
                    MenuCommand = new MyCommond<string>(OnButtonClick)
                },
                new MenuItem()
                {
                    MenuName = "Send Command",
                    MenuData = SendCmd,
                    MenuStyle = headSetResource["StyleMainButton"] as Style,
                    MenuEnabled = true,
                    MenuCommand = new MyCommond<string>(OnButtonClick)
                }
            };
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
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleLEDRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = "Repeat forward",
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleLEDRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = "Back and Forth",
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleLEDRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = "Lookup Table",
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleLEDRadioBtn"] as Style
                        }
                    },
                    ChildVisble = Visibility.Visible,
                    ChildItems = new ObservableCollection<IMenuItem>()
                    {
                        new MenuItem()
                        {
                            MenuName = "Red",
                            MenuData = "CheckBox",
                            MenuStyle = headSetResource["StyleCheckBox"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = "Blue",
                            MenuData = "CheckBox",
                            MenuStyle = headSetResource["StyleCheckBox"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = "Green",
                            MenuData = "CheckBox",
                            MenuStyle = headSetResource["StyleCheckBox"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = "Black",
                            MenuData = "CheckBox",
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
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleFanRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = "Light",
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleFanRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = "Medium",
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleFanRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = "Heavy",
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleFanRadioBtn"] as Style
                        }
                    },
                    ChildVisble = Visibility.Collapsed
                }
            };
        }

        private void OnButtonClick(string obj)
        {
            if (obj.Equals(OpenCmd))
            {

            }
            if (obj.Equals(CloseCmd))
            {

            }
            if (obj.Equals(SendCmd))
            {

            }
        }
    }

    /// <summary>
    /// The DataContext of Main items
    /// </summary>
    class MainItemDC
    {
        public Visibility ChildVisble { get; set; }
        public ObservableCollection<IMenuItem> TitleStrings { get; set; }
        public ObservableCollection<IMenuItem> SubItems { get; set; }
        public ObservableCollection<IMenuItem> ChildItems { get; set; }
    }
}
