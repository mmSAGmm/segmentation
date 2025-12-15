using Durak.DataAccess.Abtractions;
using Durak.DataAccess.Implementation;
using Durak.DomainModels;
using Durak.DomainModels.GameEngine.Implementation;
using Durak.Engine.Domain.Abtractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.Engine.Domain.Implementation
{
    public class GameService(IGameRepository gameRepository) : IGameService
    {
        public async Task<Guid> CreateGame(int playersCount)
        {
            var players = Enumerable
                .Range(0, playersCount)
                .Select(x => new Player(x.ToString()))
                .ToList();

            var game = new MultiplayerGame(players, new Deck());
            await gameRepository.CreateGame(game);

            return game.Id;
        }

        public async Task Init()
        {
            await gameRepository.Init();
        }

        public async Task<MultiplayerGame> StartGame(Guid id)
        {
            var game = await gameRepository.GetGame(id);
            game.Start();
            await gameRepository.UpdateGame(game);
            return game;
        }

        public async Task<MultiplayerGame> TryAttack(Guid playerId, Guid gameId, Card card)
        {
            var game = await gameRepository.GetGame(gameId);
            if(game?.Attacker?.Id != playerId)
            {
                return game;
            }
            var result = game.TryAttack(card);
            await gameRepository.UpdateGame(game);
            return game;
        }

        public async Task<MultiplayerGame> TryDeffend(Guid playerId, Guid gameId, Card card)
        {
            var game = await gameRepository.GetGame(gameId);
            if (game?.Defender?.Id != playerId)
            {
                return game;
            }
            var result = game.TryDeffend(card);
            await gameRepository.UpdateGame(game);
            return game;
        }

        public async Task<MultiplayerGame> TryEndRound(Guid playerId, Guid gameId)
        {
            var game = await gameRepository.GetGame(gameId);
            var result = game.TryEndRound();
            await gameRepository.UpdateGame(game);
            return game;
        }
    }
}
