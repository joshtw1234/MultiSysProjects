using PokeGameModule.Games.Enums;
using PokeGameModule.Games.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeGameModule.Games
{
    class PokeEntry
    {
        private static PokeEntry _instence;
        public static PokeEntry Instence
        {
            get
            {
                if (_instence == null)
                {
                    _instence = new PokeEntry();
                }
                return _instence;
            }
        }

        const int CardLength = 13;
        PokeCard[] listBaseCard;
        List<PokeGamer> listGamer;
        public void SpendCard()
        {
            GetBasePokeCard();
            ShuffulCard();
            SpendCardToGamer();
            printOutAllCards();
        }

        private void printOutAllCards()
        {
            foreach(var gamer in listGamer)
            {
                var sorted = gamer.GamerCards.OrderBy(x=>x.CardValue);
                foreach(var cards in sorted)
                {
                    Console.WriteLine($"Gamer {gamer.Gamer} {cards.CardFlower}+{cards.CardValue}");
                }
            }
        }

        private void SpendCardToGamer()
        {
            listGamer = new List<PokeGamer>() {
                new PokeGamer() { Gamer = GamerID.A, GamerCards = new List<PokeCard>()},
                new PokeGamer() { Gamer = GamerID.B, GamerCards = new List<PokeCard>()},
                new PokeGamer() { Gamer = GamerID.C, GamerCards = new List<PokeCard>()},
                new PokeGamer() { Gamer = GamerID.D, GamerCards = new List<PokeCard>()}};

            for (int i = 0; i < 52; i++)
            {
                if (i % 4 == 0)
                    listGamer[3].GamerCards.Add(listBaseCard[i]);
                else if (i % 4 == 1)
                    listGamer[2].GamerCards.Add(listBaseCard[i]);
                else if (i % 4 == 2)
                    listGamer[1].GamerCards.Add(listBaseCard[i]);
                else if (i % 4 == 3)
                    listGamer[0].GamerCards.Add(listBaseCard[i]);
            }
        }

        private void GetBasePokeCard()
        {
            listBaseCard = new PokeCard[CardLength * 4];
            for (int i = 0; i < Enum.GetNames(typeof(PokeCardFlower)).Length; i++)
            {
                for (uint j = 1; j < 14; j++)
                {
                    listBaseCard[i * CardLength + j - 1] = new PokeCard() { CardFlower = (PokeCardFlower)i, CardValue = j };
                }
            }
        }

        private void ShuffulCard()
        {
            for (int i = 1; i <= listBaseCard.Length; i++)
            {
                Random random = new Random();
                int num = random.Next(0, 53);
                PokeCard temp = listBaseCard[i - 1];
                listBaseCard[i - 1] = listBaseCard[num - 1];
                listBaseCard[num - 1] = temp;
            }
        }
    }
}
