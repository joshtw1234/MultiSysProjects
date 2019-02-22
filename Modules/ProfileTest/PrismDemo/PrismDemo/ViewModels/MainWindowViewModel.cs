using PrismDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismDemo.ViewModels
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
