using Durak.Engine.Domain.GameEngine.Abtractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.Engine.Domain.GameEngine.Implementation
{
    public class MultiplayerGame(List<Player> players) : IGame
    {
        private Guid Id { get; set; } = Guid.NewGuid();

        private Deck deck = new Deck();

        private List<Card> discard = new(52);

        private Player attacker;

        private Player defender;

        private GameState state;

        private Stack<Card> activeStack = new();

        public void Start()
        {
            attacker = players[0];
            defender = players[1];
            foreach (var player in players)
            {
                player.DrawCards(deck);
            }
            MoveGameState(GameState.PendingAttack);
        }

        public void Attack(Card attackCard)
        {
            if (state == GameState.PendingAttack
                && attackCard is not null
                && TryAttack(attackCard)
                && attacker.TryTakeCard(attackCard))
            {
                activeStack.Push(attackCard);
                MoveGameState(GameState.PendingDefence);
            }
        }

        public bool TryDeffend(Card defendCard)
        {
            if (state == GameState.PendingDefence
                && activeStack.TryPeek(out var attackCard)
                && CanBeat(attackCard, defendCard)
                && defender.TryTakeCard(defendCard))
            {
                MoveGameState(GameState.PendingAttack);
                return true;
            }

            return false;
        }

        public void RoundEnd()
        {
            DiscardActiveStack();
            RefillCards();
            CleanupEmplyUsers();
            MoveToNextPlayer();
            MoveGameState(NextState());
        }

        private GameState NextState() 
        {
            return players.Count > 1 ? GameState.PendingAttack : GameState.Completed;
        }

        private void DiscardActiveStack()
        {
            discard.AddRange(activeStack);
            activeStack.Clear();
        }

        private bool CanBeat(Card attackCard, Card defendCard)
        {
            if (attackCard == null || defendCard == null)
            {
                return false;
            }

            var trump = deck.TrumpSuit;
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

        private bool TryAttack(Card attackCard)
        {
            if (!activeStack.Any() || activeStack.Any(x => x.Rank == attackCard.Rank))
            {
                return true;
            }

            return false;
        }

        private void MoveToNextPlayer()
        {
            attacker = defender;
            var indexOfDefender = players.IndexOf(defender);
            if (indexOfDefender == players.Count() - 1)
            {
                indexOfDefender = 0;
            }
            else
            {
                indexOfDefender++;
            }
            defender = players.ElementAt(indexOfDefender);
        }
        private void MoveGameState(GameState newState) => state = newState;

        private void CleanupEmplyUsers() 
        {
            var toRemove = players.Where(x => !x.Hand.Any());
            foreach (var player in toRemove)
            {
                players.Remove(player);
            }
        }

        private void RefillCards() 
        {
            attacker.DrawCards(deck);
            defender.DrawCards(deck);
        }
    }
}
