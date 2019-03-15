using PokeGameModule.Games.Enums;
using System.Collections.Generic;

namespace PokeGameModule.Games.Structures
{
    class PokeGamer
    {
        public GamerID Gamer;
        public List<PokeCard> GamerCards { get; set; }
    }
}
