using MenuModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuModule.ViewModels
{
    class MenuControlViewModel
    {
        IMenuControlModel _model;
        public MenuControlViewModel(IMenuControlModel model)
        {
            _model = model;
        }
    }
}
