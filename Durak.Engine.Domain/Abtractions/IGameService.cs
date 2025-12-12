using Durak.DomainModels;
using Durak.DomainModels.GameEngine.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.Engine.Domain.Abtractions
{
    public interface IGameService
    {
        Task<Guid> CreateGame(int playersCount);

        Task<MultiplayerGame> StartGame(Guid id);

        Task<MultiplayerGame> TryAttack(Guid playerId, Guid gameId, Card card);

        Task<MultiplayerGame> TryDeffend(Guid playerId, Guid gameId, Card card);

        Task<MultiplayerGame> TryEndRound(Guid playerId, Guid gameId);
    }
}
