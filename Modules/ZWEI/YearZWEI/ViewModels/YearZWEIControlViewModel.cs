using YearZWEI.Models;

namespace YearZWEI.ViewModels
{
    class YearZWEIControlViewModel
    {
        IYearZWEIModel zWeiModel;
        public YearZWEIControlViewModel(IYearZWEIModel yzWeiModel)
        {
            zWeiModel = yzWeiModel;
        }
    }
}
