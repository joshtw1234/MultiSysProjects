using System.Windows;

namespace YearZWEI.Models
{
    interface IYearZWEIModel
    {
        bool Initialize();

        ResourceDictionary GetResourceDic();
    }
}
