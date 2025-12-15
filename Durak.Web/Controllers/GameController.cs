using Durak.DomainModels;
using Durak.DomainModels.GameEngine.Abtractions;
using Durak.DomainModels.GameEngine.Implementation;
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
        public async Task<IActionResult> StartGame(Guid gameId)
        {
            var game = await gameService.StartGame(gameId);
            if (game != null)
            {
                return Ok(game);
            }
            return BadRequest();
        }

        [HttpGet("state")]
        public async Task<Player> GetState(Guid playerId, Guid gameId)
        {
            var game = await gameService.Get(gameId);
            return game?.Players?.FirstOrDefault(x => x.Id == playerId);
        }

        [HttpPost("attack")]
        public async Task<IActionResult> Attack(Guid playerId, Guid gameId, Card card)
        {
            var game = await gameService.TryAttack(playerId, gameId, card);
            if (game == null) return BadRequest();
            return Ok(game);
        }

        [HttpPost("defend")]
        public async Task<IActionResult> Defend(Guid playerId, Guid gameId, Card card)
        {
            var game = await gameService.TryDeffend(playerId, gameId, card);
            if (game == null) return BadRequest();
            return Ok(game);
        }

        [HttpPost("end")]
        public async Task<IActionResult> EndRound(Guid playerId, Guid gameId, Card card)
        {
            var game = await gameService.TryEndRound(playerId, gameId);
            if (game == null) return BadRequest();
            return Ok(game);
        }
    }
}
