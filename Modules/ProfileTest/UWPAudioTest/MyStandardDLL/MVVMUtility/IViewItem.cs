using System.Windows;
using Windows.UI.Xaml;

namespace MyStandardDLL.MVVMUtility
{
    /// <summary>
    /// This Interface present a common View properties.
    /// </summary>
    public interface IViewItem
    {
        /// <summary>
        /// MenuName is the name associated to a particular menu item
        /// </summary>
        string MenuName { get; set; }

        /// <summary>
        /// MenuImage is the image (.png) associated to a particular menu item
        /// </summary>
        string MenuImage { get; set; }

        /// <summary>
        /// MenuImage is the image (.png) associated to a particular menu item when pressed
        /// </summary>
        string MenuImagePressed { get; set; }

        /// <summary>
        /// MenuData is the data associated to a particular menu item
        /// </summary>
        string MenuData { get; set; }

        /// <summary>
        /// MenuCommand is the command that is executed for an action
        /// on a particular menu item 
        /// </summary>
        //DelegateCommand<string> MenuCommand { get; set; }
        MyDelegateCommond<string> MenuCommand { get; set; }

        /// <summary>
        /// MenuStyle is the styling applied to a particular menu item
        /// </summary>
        Style MenuStyle { get; set; }

        /// <summary>
        /// MenuEnabled sets whether a particular menu item is enabled or disabled
        /// </summary>
        bool MenuEnabled { get; set; }

        /// <summary>
        /// MenuChecked sets whether a particular menu item is checked or not
        /// </summary>
        bool MenuChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether menu is visible or not
        /// </summary>
        bool MenuVisibility { get; set; }
    }
}
