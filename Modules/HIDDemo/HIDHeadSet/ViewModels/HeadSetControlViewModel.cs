using HIDHeadSet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using UtilityUILib;

namespace HIDHeadSet.ViewModels
{
    class HeadSetControlViewModel : BindAbleBases
    {
        const string OpenCmd= "Open";
        const string CloseCmd = "Close";
        const string LEDCmd = "LED";
        const string FanCmd = "Fan";

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
            if (!headSetModel.Initialize())
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
                    MenuName = CloseCmd,
                    MenuData = CloseCmd,
                    MenuStyle = headSetResource["StyleMainButton"] as Style,
                    MenuEnabled = true,
                    MenuCommand = new MyCommond<string>(OnButtonClick)
                },
                new MenuItem()
                {
                    MenuName = "LED Command",
                    MenuData = LEDCmd,
                    MenuStyle = headSetResource["StyleMainButton"] as Style,
                    MenuEnabled = true,
                    MenuCommand = new MyCommond<string>(OnButtonClick)
                },
                new MenuItem()
                {
                    MenuName = "Fan Command",
                    MenuData = FanCmd,
                    MenuStyle = headSetResource["StyleMainButton"] as Style,
                    MenuEnabled = true,
                    MenuCommand = new MyCommond<string>(OnButtonClick)
                }
            };
        }

        private ObservableCollection<MainItemDC> GetMainItems()
        {
            var rev = new ObservableCollection<MainItemDC>();
            var m1 = new MainItemDC()
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
                            MenuName = HeadSetConstants.LEDStatic,
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleLEDRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = HeadSetConstants.LEDRepeatForward,
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleLEDRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = HeadSetConstants.LEDBackForth,
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleLEDRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = HeadSetConstants.LEDLookupTable,
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleLEDRadioBtn"] as Style
                        }
                    },
                ChildVisble = Visibility.Visible,
                ColorInterVal = new MenuItem()
                {
                    MenuName = "100",
                    MenuStyle = headSetResource["StyleColorInterVal"] as Style
                }
            };

            var values = typeof(Brushes).GetProperties().Select(p => new { Name = p.Name, Brush = p.GetValue(null) as Brush }).ToArray();
            m1.ChildItems = new ObservableCollection<IMenuItem>();
            foreach(var vv in values)
            {
                m1.ChildItems.Add(new BrushMenuItem()
                {
                    MenuName = vv.Name,
                    MenuBrush = vv.Brush,
                    MenuData = "CheckBox",
                    MenuStyle = headSetResource["StyleCheckBox"] as Style
                });
            }
            rev.Add(m1);
            var m2 = new MainItemDC()
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
                            MenuName = HeadSetFanModes.Off.ToString(),
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleFanRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = HeadSetFanModes.Light.ToString(),
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleFanRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = HeadSetFanModes.Medium.ToString(),
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleFanRadioBtn"] as Style
                        },
                        new MenuItem()
                        {
                            MenuName = HeadSetFanModes.Heavy.ToString(),
                            MenuData = "RadioButton",
                            MenuStyle = headSetResource["StyleFanRadioBtn"] as Style
                        }
                    },
                ChildVisble = Visibility.Collapsed
            };
            rev.Add(m2);
            return rev;
        }

        private void OnButtonClick(string obj)
        {
            string LEDMode = string.Empty;
            string FanMode = string.Empty;
            if (obj.Equals(CloseCmd))
            {
                headSetModel.CloseHID();
            }
            if (obj.Equals(LEDCmd))
            {
                //LED Control
                foreach (var subItem in mainItems[0].SubItems)
                {
                    if (subItem.MenuChecked)
                    {
                        LEDMode = subItem.MenuName;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(LEDMode))
                {
                    //Get Color
                    List<Brush> lstBrush = new List<Brush>();
                    foreach (var childItem in mainItems[0].ChildItems)
                    {
                        if (childItem.MenuChecked)
                        {
                            lstBrush.Add((childItem as BrushMenuItem).MenuBrush);
                        }
                    }
                    headSetModel.SetColorData(LEDMode, lstBrush, ushort.Parse(mainItems[0].ColorInterVal.MenuName));
                }
                
            }
            if (obj.Equals(FanCmd))
            {
#if true
                headSetModel.GetFWInfo();
#else
                //Fan Control
                foreach (var subItem in mainItems[1].SubItems)
                {
                    if (subItem.MenuChecked)
                    {
                        FanMode = subItem.MenuName;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(FanMode))
                {
                    HeadSetFanModes fMode = (HeadSetFanModes)Enum.Parse(typeof(HeadSetFanModes), FanMode);
                    headSetModel.SetFanData(fMode);
                }
#endif
            }
        }
    }

    class BrushMenuItem : MenuItem
    {
        public Brush MenuBrush { get; set; }
    }

    /// <summary>
    /// The DataContext of Main items
    /// </summary>
    class MainItemDC
    {
        //Here no need bindable, view could use binding updatesourcetrigger to change the value.
        //but model can not update UI from here at runtime.
        public Visibility ChildVisble { get; set; }
        public IMenuItem ColorInterVal { get; set; }
        public ObservableCollection<IMenuItem> TitleStrings { get; set; }
        public ObservableCollection<IMenuItem> SubItems { get; set; }
        public ObservableCollection<IMenuItem> ChildItems { get; set; }
    }
}
