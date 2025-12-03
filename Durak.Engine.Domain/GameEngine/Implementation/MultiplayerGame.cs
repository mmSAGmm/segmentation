using Durak.Engine.Domain.GameEngine.Abtractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.Engine.Domain.GameEngine.Implementation
{
    public class MultiplayerGame : IGame
    {
        public MultiplayerGame(List<Player> players, Deck deck)
        {
            this.players = players;
            this.deck = deck;
        }
        private Guid Id { get; set; } = Guid.NewGuid();

        internal List<Player> players;
        
        internal Deck deck;

        private List<Card> discard = new(52);

        public Player? Attacker { get; set; }

        public Player? Defender { get; set; }

        internal GameState state;

        private Stack<Card> activeStack = new();

        public void Start()
        {
            Attacker = players[0];
            Defender = players[1];
            foreach (var player in players)
            {
                player.DrawCards(deck);
            }
            MoveGameState(GameState.PendingAttack);
        }

        public bool TryAttack(Card attackCard)
        {
            if (state == GameState.PendingAttack
                && attackCard is not null
                && CanAttack(attackCard)
                && Attacker.TryTakeCard(attackCard))
            {
                activeStack.Push(attackCard);
                MoveGameState(GameState.PendingDefence);
                return true;
            }

            return false;
        }

        public bool TryDeffend(Card defendCard)
        {
            if (state == GameState.PendingDefence
                && activeStack.TryPeek(out var attackCard)
                && CanBeat(attackCard, defendCard)
                && Defender.TryTakeCard(defendCard))
            {
                activeStack.Push(defendCard);
                MoveGameState(GameState.PendingAttack);
                return true;
            }

            return false;
        }

        public bool TryEndRound()
        {
            if (CanDiscard())
            {
                DiscardActiveStack();
                RefillCards();
                CleanupEmplyUsers();
                MoveToNextPlayer();
                MoveGameState(NextState());
                return true;
            }
            else if(activeStack.Any())
            {
                TakeActiveStackToDefender();
                RefillCards();
                CleanupEmplyUsers();
                MoveToNextPlayerSkipDefender();
                MoveGameState(NextState());
                return true;
            }
            return false;
        }

        private bool CanDiscard() 
        {
            return activeStack.Any() && state == GameState.PendingAttack;
        }

        private GameState NextState() 
        {
            return players.Count > 1 ? GameState.PendingAttack : GameState.Completed;
        }
        
        private void TakeActiveStackToDefender()
        {
            Defender.Hand.AddRange(activeStack);
            activeStack.Clear();
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

        private bool CanAttack(Card attackCard)
        {
            if (!activeStack.Any() || activeStack.Any(x => x.Rank == attackCard.Rank))
            {
                return true;
            }

            return false;
        }

        private void MoveToNextPlayer()
        {
            Attacker = Defender;
            var indexOfDefender = players.IndexOf(Defender);
            if (indexOfDefender == players.Count() - 1)
            {
                indexOfDefender = 0;
            }
            else
            {
                indexOfDefender++;
            }
            Defender = players.ElementAt(indexOfDefender);
        }

        private void MoveToNextPlayerSkipDefender()
        {
            var indexOfDefender = players.IndexOf(Defender);
            if (indexOfDefender == players.Count() - 1)
            {
                indexOfDefender = 0;
            }
            else
            {
                indexOfDefender++;
            }
            Attacker = players.ElementAt(indexOfDefender);

            if (indexOfDefender == players.Count() - 1)
            {
                indexOfDefender = 0;
            }
            else
            {
                indexOfDefender++;
            }

            Defender = players.ElementAt(indexOfDefender);
        }

        private void MoveGameState(GameState newState) => state = newState;

        private void CleanupEmplyUsers() 
        {
            var toRemove = players.Where(x => !x.Hand.Any()).ToList();
            foreach (var player in toRemove)
            {
                players.Remove(player);
            }
        }

        private void RefillCards() 
        {
            Attacker.DrawCards(deck);
            Defender.DrawCards(deck);
        }
    }
}
