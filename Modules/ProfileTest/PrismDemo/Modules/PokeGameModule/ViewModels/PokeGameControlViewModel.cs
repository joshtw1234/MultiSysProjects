using PokeGameModule.Games;
using PokeGameModule.Interfaces;

namespace PokeGameModule.ViewModels
{
    public class PokeGameControlViewModel
    {
        IPokeGameModel _model;
        public PokeGameControlViewModel(IPokeGameModel model)
        {
            _model = model;
            PokeEntry.Instence.SpendCard();
        }
    }
}
