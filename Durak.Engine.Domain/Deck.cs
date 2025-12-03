using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.Engine.Domain
{
    public class Deck
    {
        private readonly Queue<Card> _cards = new ();
        private readonly Random _rnd = new();

        public Suit TrumpSuit { get; }

        public Deck()
        {
            var cards = Enum.GetValues<Suit>()
                .SelectMany(s => Enum.GetValues<Rank>().Select(r => new Card(s, r)))
                .ToList();

            // Перемешиваем
            cards = cards.Shuffle().ToList();

            // Последняя карта задаёт козырь
            TrumpSuit = cards.Last().Suit;

            foreach (var card in cards)
            {
                _cards.Enqueue(card);
            }
        }

        public Card Draw() => _cards.Count > 0 ? _cards.Dequeue() : null;
        public int Count => _cards.Count;
    }
}
