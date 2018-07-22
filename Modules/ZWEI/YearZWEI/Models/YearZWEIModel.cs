using System;
using System.Windows;

namespace YearZWEI.Models
{
    class YearZWEIModel : IYearZWEIModel
    {
        ResourceDictionary zWEIResource;

        public ResourceDictionary GetResourceDic()
        {
            if (null == zWEIResource)
            {
                return zWEIResource = new ResourceDictionary()
                {
                    Source = new Uri("pack://application:,,,/YearZWEI;component/Styles/YearZWEIStyle.xaml", UriKind.RelativeOrAbsolute)
                };
            }
            return zWEIResource;
        }

        public bool Initialize()
        {
            
            return true;
        }
    }
}
