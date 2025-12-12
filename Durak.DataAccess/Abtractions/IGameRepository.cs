using Durak.DomainModels.GameEngine.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.DataAccess.Abtractions
{
    public interface IGameRepository
    {
        Task CreateGame(MultiplayerGame game);

        Task  UpdateGame(MultiplayerGame game);

        Task DeleteGame(Guid id);
    }
}
