using System;
using System.Collections.ObjectModel;
using System.Windows;
using UtilityUILib;
using YearZWEI.Models;

namespace YearZWEI.ViewModels
{
    class YearZWEIControlViewModel : BindAbleBases
    {
        IYearZWEIModel zWeiModel;
        ResourceDictionary zWEIResource;

        public YearZWEIControlViewModel(IYearZWEIModel yzWeiModel)
        {
            zWeiModel = yzWeiModel;
            zWEIResource = zWeiModel.GetResourceDic();
            zWEIFieldItem = GetZWEIFieldItems();
        }

        private ObservableCollection<IMenuItem> GetZWEIFieldItems()
        {
            return new ObservableCollection<IMenuItem>()
            {
                new MenuItem()
                {
                    MenuData="StarField",
                    MenuStyle = zWEIResource["StarFieldStyle"] as Style
                },
                new MenuItem()
                {
                    MenuData="ZWEIDesc",
                    MenuStyle = zWEIResource["DescStyle"] as Style
                }
            };
        }

        #region Properity

        private ObservableCollection<IMenuItem> zWEIFieldItem;
        public ObservableCollection<IMenuItem> ZWEIFieldItem
        {
            get
            {
                return zWEIFieldItem;
            }
            set
            {
                zWEIFieldItem = value;
                onPropertyChanged(this, "ZWEIFieldItem");
            }
        }
        #endregion
    }
}
