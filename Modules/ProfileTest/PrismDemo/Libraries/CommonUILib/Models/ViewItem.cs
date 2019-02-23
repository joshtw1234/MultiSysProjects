using CommonUILib.Interfaces;
using Prism.Commands;
using System.Windows;

namespace CommonUILib.Models
{
    public class ViewItem : IViewItem
    {
        public string MenuName { get; set; }
        public string MenuImage { get; set; }
        public string MenuImagePressed { get; set; }
        public string MenuData { get; set; }
        public DelegateCommand<string> MenuCommand { get; set; }
        public Style MenuStyle { get; set; }
        public bool MenuEnabled { get; set; }
        public bool MenuChecked { get; set; }
        public bool MenuVisibility { get; set; }
    }
}
