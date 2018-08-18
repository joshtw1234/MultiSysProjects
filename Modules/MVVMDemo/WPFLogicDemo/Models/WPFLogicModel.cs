using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFLogicDemo.Models
{
    class WPFLogicModel : IWPFLogicModel
    {
        public ResourceDictionary GetLocalStyle()
        {
            return new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/WPFLogicDemo;component/Styles/WPFLogicStyle.xaml", UriKind.RelativeOrAbsolute)
            };

        }
    }
}
