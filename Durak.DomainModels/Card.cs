using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.DomainModels
{
    public class Card
    {
        public Suit Suit { get; }
        public Rank Rank { get; }
        public Card(Suit suit, Rank rank) { Suit = suit; Rank = rank; }
        public override string ToString() => $"{Rank} of {Suit}";
    }
}
