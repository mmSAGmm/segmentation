using System;
using System.Collections.Generic;
using System.Linq;

namespace Durak.Engine.Domain
{
    public class Game
    {
        private readonly Deck _deck = new();
        private readonly Player _p1;
        private readonly Player _p2;
        private Player _attacker;
        private Player _defender;
        private readonly List<Card> _discard = new();

        public Game(string p1Name, string p2Name)
        {
            _p1 = new Player(p1Name);
            _p2 = new Player(p2Name);

            _p1.DrawCards(_deck);
            _p2.DrawCards(_deck);

            _attacker = _p1;
            _defender = _p2;

            Console.WriteLine($"Trump suit: {_deck.TrumpSuit}");
        }

        public void Start()
        {
            while (HasCardsInPlay())
            {
                Console.WriteLine("\n--- New Round ---");
                Console.WriteLine($"Attacker: {_attacker.Name}, Defender: {_defender.Name}");
                _attacker.ShowHand();
                Console.WriteLine("Choose card to attack (index):");
                if (!TryGetAttackCard(out var attackCard))
                {
                    Console.WriteLine("Invalid selection, skipping turn.");
                    continue;
                }

                Console.WriteLine($"{_attacker.Name} attacks with {attackCard}");
                _attacker.Hand.Remove(attackCard);

                var defendCard = ChooseDefense(attackCard);
                if (defendCard != null)
                {
                    Console.WriteLine($"{_defender.Name} defends with {defendCard}");
                    _defender.Hand.Remove(defendCard);
                    MoveToDiscard(attackCard, defendCard);
                    SwapRoles();
                }
                else
                {
                    Console.WriteLine($"{_defender.Name} cannot defend, takes card");
                    _defender.Hand.Add(attackCard);
                }

                RefillHands();
            }

            Console.WriteLine("Game over!");
            Console.WriteLine(_p1.Hand.Count == 0 && _deck.Count == 0
                ? $"{_p1.Name} wins!"
                : _p2.Hand.Count == 0 && _deck.Count == 0
                    ? $"{_p2.Name} wins!"
                    : "Draw");
        }

        private bool HasCardsInPlay() =>
            _deck.Count > 0 || _p1.Hand.Count > 0 || _p2.Hand.Count > 0;

        private bool TryGetAttackCard(out Card attackCard)
        {
            attackCard = null;
            if (!int.TryParse(Console.ReadLine(), out int idx))
            {
                return false;
            }

            if (idx <= 0 || idx > _attacker.Hand.Count)
            {
                return false;
            }

            attackCard = _attacker.Hand[idx - 1];
            return true;
        }

        private Card? ChooseDefense(Card attackCard)
        {
            var defendCard = _defender.Hand
                .OrderBy(card => card.Suit == _deck.TrumpSuit ? 0 : 1)
                .ThenBy(card => card.Rank)
                .FirstOrDefault(card => CanBeat(attackCard, card));

            return defendCard;
        }

        private bool CanBeat(Card attackCard, Card defendCard)
        {
            if (attackCard == null || defendCard == null)
            {
                return false;
            }

            var trump = _deck.TrumpSuit;
            if (defendCard.Suit == attackCard.Suit)
            {
                return defendCard.Rank > attackCard.Rank;
            }

            if (defendCard.Suit == trump && attackCard.Suit != trump)
            {
                return true;
            }

            return false;
        }

        private void SwapRoles()
        {
            (_attacker, _defender) = (_defender, _attacker);
        }

        private void MoveToDiscard(params Card[] cards)
        {
            foreach (var card in cards)
            {
                if (card != null)
                {
                    _discard.Add(card);
                }
            }
        }

        private void RefillHands()
        {
            // According to rules attacker draws first, then defender
            _attacker.DrawCards(_deck);
            _defender.DrawCards(_deck);
        }
    }
}
