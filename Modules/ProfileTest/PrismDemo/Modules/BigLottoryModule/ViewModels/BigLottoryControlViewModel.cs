using BigLottoryModule.Interface;

namespace BigLottoryModule.ViewModels
{
    class BigLottoryControlViewModel
    {
        private IBigLottoryControlModel _model;
        public BigLottoryControlViewModel(IBigLottoryControlModel model)
        {
            _model = model;
        }
    }
}
