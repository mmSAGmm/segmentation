using Durak.DomainModels;
using Durak.Engine.Domain;
using Durak.Engine.Domain.Abtractions;
using Microsoft.AspNetCore.Mvc;

namespace Durak.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GameController(IGameService gameService) : ControllerBase
    {
        [HttpPost("init")]
        public async Task Init()
        {
            await gameService.Init();
        }

        [HttpPost("create")]
        public async Task<Guid> CreateGame(int playersCount)
        {
            return await gameService.CreateGame(playersCount);
        }

        [HttpPost("start")]
        public async Task StartGame(Guid gameId)
        {
            await gameService.StartGame(gameId);
        }

        [HttpPost("attack")]
        public async Task Attack(Guid playerId, Guid gameId, Card card)
        {
            await gameService.TryAttack(playerId, gameId, card);
        }

        [HttpPost("defend")]
        public async Task Defend(Guid playerId, Guid gameId, Card card)
        {
            await gameService.TryDeffend(playerId, gameId, card);
        }

        [HttpPost("end")]
        public async Task EndRound(Guid playerId, Guid gameId, Card card)
        {
            await gameService.TryEndRound(playerId, gameId);
        }
    }
}
