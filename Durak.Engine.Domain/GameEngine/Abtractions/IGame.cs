using System;
using System.Collections.Generic;
using System.Text;

namespace Durak.Engine.Domain.GameEngine.Abtractions
{
    public interface IGame
    {
        public void Start();

        public void Attack(Card card);

        public bool TryDeffend(Card action);

        public void RoundEnd();
    }
}
