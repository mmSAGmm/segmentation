using Durak.DomainModels.GameEngine.Abtractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.DomainModels.GameEngine.Implementation
{
    public class MultiplayerGame : IGame
    {
        public MultiplayerGame(List<Player> players, Deck deck)
        {
            this.Players = players;
            this.Deck = deck;
        }
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<Player> Players { get; set; }

        public Deck Deck { get; set; }

        public List<Card> Discard { get; set; } = new(52);

        public Player? Attacker { get; set; }

        public Player? Defender { get; set; }

        public GameState State { get; set; }

        public Stack<Card> ActiveStack { get; set; } = new();

        public void Start()
        {
            Attacker = Players[0];
            Defender = Players[1];
            foreach (var player in Players)
            {
                player.DrawCards(Deck);
            }
            MoveGameState(GameState.PendingAttack);
        }

        public bool TryAttack(Card attackCard)
        {
            if (State == GameState.PendingAttack
                && attackCard is not null
                && CanAttack(attackCard)
                && Attacker.TryTakeCard(attackCard))
            {
                ActiveStack.Push(attackCard);
                MoveGameState(GameState.PendingDefence);
                return true;
            }

            return false;
        }

        public bool TryDeffend(Card defendCard)
        {
            if (State == GameState.PendingDefence
                && ActiveStack.TryPeek(out var attackCard)
                && CanBeat(attackCard, defendCard)
                && Defender.TryTakeCard(defendCard))
            {
                ActiveStack.Push(defendCard);
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
            else if(ActiveStack.Any())
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
            return ActiveStack.Any() && State == GameState.PendingAttack;
        }

        private GameState NextState() 
        {
            return Players.Count > 1 ? GameState.PendingAttack : GameState.Completed;
        }
        
        private void TakeActiveStackToDefender()
        {
            Defender.Hand.AddRange(ActiveStack);
            ActiveStack.Clear();
        }

        private void DiscardActiveStack()
        {
            Discard.AddRange(ActiveStack);
            ActiveStack.Clear();
        }

        private bool CanBeat(Card attackCard, Card defendCard)
        {
            if (attackCard == null || defendCard == null)
            {
                return false;
            }

            var trump = Deck.TrumpSuit;
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
            if (!ActiveStack.Any() || ActiveStack.Any(x => x.Rank == attackCard.Rank))
            {
                return true;
            }

            return false;
        }

        private void MoveToNextPlayer()
        {
            Attacker = Defender;
            var indexOfDefender = Players.IndexOf(Defender);
            if (indexOfDefender == Players.Count() - 1)
            {
                indexOfDefender = 0;
            }
            else
            {
                indexOfDefender++;
            }
            Defender = Players.ElementAt(indexOfDefender);
        }

        private void MoveToNextPlayerSkipDefender()
        {
            var indexOfDefender = Players.IndexOf(Defender);
            if (indexOfDefender == Players.Count() - 1)
            {
                indexOfDefender = 0;
            }
            else
            {
                indexOfDefender++;
            }
            Attacker = Players.ElementAt(indexOfDefender);

            if (indexOfDefender == Players.Count() - 1)
            {
                indexOfDefender = 0;
            }
            else
            {
                indexOfDefender++;
            }

            Defender = Players.ElementAt(indexOfDefender);
        }

        private void MoveGameState(GameState newState) => State = newState;

        private void CleanupEmplyUsers() 
        {
            var toRemove = Players.Where(x => !x.Hand.Any()).ToList();
            foreach (var player in toRemove)
            {
                Players.Remove(player);
            }
        }

        private void RefillCards() 
        {
            Attacker.DrawCards(Deck);
            Defender.DrawCards(Deck);
        }
    }
}
