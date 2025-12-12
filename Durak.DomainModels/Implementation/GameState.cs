using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.DomainModels.GameEngine.Implementation
{
    public enum GameState
    {
        Created = 0,
        PendingAttack = 10,
        PendingDefence = 20,
        Completed = 3
    }
}
