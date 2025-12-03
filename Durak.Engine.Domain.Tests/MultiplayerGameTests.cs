using Durak.Engine.Domain.GameEngine.Implementation;
using Moq.AutoMock;
using Shouldly;
using System.Collections.Generic;

namespace Durak.Engine.Domain.Tests
{
    public class MultiplayerGameTests
    {
        private AutoMocker mocker = new AutoMocker();

        private MultiplayerGame _subject;
        public MultiplayerGame Subject => _subject ??= mocker.CreateInstance<MultiplayerGame>();

        private readonly Card[] cards = new Card[] {
                new(Suit.Spades, Rank.Six),
                new(Suit.Hearts, Rank.Six),//1
                new(Suit.Hearts, Rank.Seven),//2
                new(Suit.Hearts, Rank.Eight),//3
                new(Suit.Hearts, Rank.Nine),//4
                new(Suit.Hearts, Rank.Ten),//5
                new(Suit.Hearts, Rank.Jack),//6
                new(Suit.Hearts, Rank.Queen),//7
                new(Suit.Hearts, Rank.King),//8
                new(Suit.Hearts, Rank.Ace),//9
                new(Suit.Clubs, Rank.Six),//10
                new(Suit.Clubs, Rank.Seven),//11
                new(Suit.Clubs, Rank.Eight)//12
            };
        private readonly List<Player> players = new List<Player>() { new("Jack"), new("Vasia") };

        public void WithUsers(List<Player> players) 
        {
            mocker.Use<List<Player>>(players);
        }


        public void WithDeck(Queue<Card> cards, Suit suit)
        {
            mocker.Use<Deck>(new Deck(cards, suit));
        }

        [Fact]
        public void WhenPlaySimpleRoundWithDiscard()
        {
            WithDeck(ToQueue(cards), Suit.Hearts);
            WithUsers(players);
            
            Subject.state.ShouldBe(GameState.Created);
            Subject.Start();
            
            Subject.state.ShouldBe(GameState.PendingAttack);

            var attacker = Subject.Attacker;
            var defender = Subject.Defender;
         
            Subject.TryAttack(Subject.Attacker.Hand.First())
                .ShouldBe(true);

            Subject.state.ShouldBe(GameState.PendingDefence);
           
            Subject.TryDeffend(Subject.Defender.Hand.First())
                .ShouldBe(true);

            Subject.state.ShouldBe(GameState.PendingAttack);

            Subject.TryEndRound()
                .ShouldBeTrue();

            Subject.Attacker.Hand.Count.ShouldBe(5);
            Subject.Defender.Hand.Count.ShouldBe(6);
            ShouldSwapPlayers(attacker, defender);
            Subject.state.ShouldBe(GameState.PendingAttack);
        }

        [Fact]
        public void WhenAttackSeveralTimes()
        {
            WithDeck(ToQueue(cards), Suit.Hearts);
            WithUsers(players);

            Subject.Start();

            Subject.TryAttack(Subject.Attacker.Hand.First())
                .ShouldBe(true);

            Subject.TryDeffend(Subject.Defender.Hand.First(x=>x.Suit == Subject.deck.TrumpSuit))
                .ShouldBe(true);

            Subject.TryAttack(Subject.Attacker.Hand.First())
                .ShouldBe(true);

            Subject.TryDeffend(Subject.Defender.Hand.First(x => x.Suit == Subject.deck.TrumpSuit))
                .ShouldBe(true);
            Subject.TryEndRound().ShouldBeTrue();

            Subject.Attacker.Hand.Count.ShouldBe(4);
            Subject.Defender.Hand.Count.ShouldBe(5);
        }

        [Fact]
        public void WhenPlaySimpleRoundWithDefenderTake()
        {
            WithDeck(ToQueue(cards), Suit.Hearts);
            WithUsers(players);

            Subject.Start();

            var attacker = Subject.Attacker;
            var defender = Subject.Defender;

            Subject.TryAttack(Subject.Attacker.Hand.First())
                .ShouldBe(true);
            Subject.TryEndRound()
                .ShouldBeTrue();

            ShouldSkipDefender(attacker, defender);
            Subject.state.ShouldBe(GameState.PendingAttack);
        }

        private void ShouldSwapPlayers(Player oldAttacker, Player oldDefender)
        {
            Subject.Attacker.ShouldBe(oldDefender);
            Subject.Defender.ShouldBe(oldAttacker);
        }

        private void ShouldSkipDefender(Player oldAttacker, Player oldDefender)
        {
            oldAttacker.ShouldBe(Subject.Attacker);
            oldDefender.ShouldBe(Subject.Defender);
        }

        private Queue<Card> ToQueue(Card[] cards)
        {
            Queue<Card> queue = new();
            foreach (var card in cards)
            {
                queue.Enqueue(card);
            }
            return queue;
        }
    }
}