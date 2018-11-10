using DirectShowDemo.Models;

namespace DirectShowDemo.ViewModels
{
    class MainWindowViewModel
    {
        IMainWindowModel _model;
        public MainWindowViewModel(IMainWindowModel model)
        {
            _model = model;
        }
    }
}
