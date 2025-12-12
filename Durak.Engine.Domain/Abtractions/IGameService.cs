using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.Engine.Domain.Abtractions
{
    public interface IGameService
    {
        Task CreateGame(int playersCount);
    }
}
