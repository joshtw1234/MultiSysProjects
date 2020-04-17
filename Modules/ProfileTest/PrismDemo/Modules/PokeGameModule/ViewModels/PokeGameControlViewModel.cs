using PokeGameModule.Games;
using PokeGameModule.Interfaces;
using Prism.Mvvm;

namespace PokeGameModule.ViewModels
{
    public class PokeGameControlViewModel : BindableBase
    {
        IPokeGameModel _model;
        public PokeGameControlViewModel(IPokeGameModel model)
        {
            _model = model;
            PokeEntry.Instence.SpendCard();
        }
        private string _testValid;
        public string TestValid
        {
            get
            {
                return _testValid;
            }
            set
            {
                SetProperty(ref _testValid, value);
            }
        }
    }
}
