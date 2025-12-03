using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.Engine.Domain.GameEngine.Abtractions
{
    public interface IGame
    {
        public void Start();

        public bool TryAttack(Card card);

        public bool TryDeffend(Card action);

        public bool TryEndRound();
    }
}
