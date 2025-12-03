using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.Engine.Domain
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; }
        public List<Card> Hand { get; } = new();

        public Player(string name) { Name = name; }

        public void DrawCards(Deck deck, int targetCount = 6)
        {
            while (Hand.Count < targetCount && deck.Count > 0)
                Hand.Add(deck.Draw());
        }

        public void ShowHand()
        {
            Console.WriteLine($"{Name}'s hand:");
            for (int i = 0; i < Hand.Count; i++)
                Console.WriteLine($"{i + 1}: {Hand[i]}");
        }

        public bool TryTakeCard(Card attackCard)
        {
            var card = Hand.FirstOrDefault(x => x == attackCard);
            if(card != null)
            {
                Hand.Remove(card);
            }
            return false;
        }
    }
}
