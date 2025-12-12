using Durak.DataAccess.Abtractions;
using Durak.Engine.Domain.Abtractions;
using Durak.Engine.Domain.GameEngine.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.Engine.Domain.Implementation
{
    public class GameService(IGameRepository gameRepository) : IGameService
    {
        public async Task CreateGame(int playersCount)
        {
            var players = Enumerable
                .Range(0, playersCount)
                .Select(x => new Player(x.ToString()))
                .ToList();

            var game = new MultiplayerGame(players, new Engine.Domain.Deck());
        }
    }
}
