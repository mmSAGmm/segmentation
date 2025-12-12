using Durak.Engine.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Durak.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GameController : ControllerBase
    {
        public async Task CreateGame(int playersCount)
        {
           
        }
    }
}
