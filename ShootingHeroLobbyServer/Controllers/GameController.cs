using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShootingHero.LobbyServer
{
    [ApiController]
    [Route("Game")]
    public class GameController : ControllerBase
    {
        private readonly GameInstanceManager gameInstanceManager = null;

        public GameController(GameInstanceManager gameInstanceManager)
        {
            this.gameInstanceManager = gameInstanceManager;
        }

        [HttpPost(nameof(GetGameConnectionRequest))]
        public async Task<ActionResult<GetGameConnectionResponse>> GetGameConnectionPost([FromBody]GetGameConnectionRequest request)
        {
            if(string.IsNullOrEmpty(request.GameUUID) == true)
                return new GetGameConnectionResponse();

            if(gameInstanceManager.TryGetGameInstanceHandle(request.GameUUID, out GameInstanceHandle gameInstanceHandle) == false)
                return new GetGameConnectionResponse();
            
            if(gameInstanceHandle == null)
                return new GetGameConnectionResponse();
            
            TimeSpan timeout = TimeSpan.FromSeconds(30);
            DateTime expireAt = DateTime.UtcNow + timeout;

            while(DateTime.UtcNow < expireAt)
            {
                if(gameInstanceHandle.IsReady == false)
                {
                    await Task.Delay(500);
                    continue;
                }

                return new GetGameConnectionResponse() { 
                    Host = gameInstanceHandle.Host,
                    Port = gameInstanceHandle.Port
                };
            }

            return new GetGameConnectionResponse();
        }

        [HttpPost(nameof(GameInstanceReadyRequest))]
        public ActionResult<GameInstanceReadyResponse> GameInstanceReadyPost([FromBody]GameInstanceReadyRequest request)
        {
            if(string.IsNullOrEmpty(request.GameUUID) == true)
                return new GameInstanceReadyResponse();

            if(gameInstanceManager.TryGetGameInstanceHandle(request.GameUUID, out GameInstanceHandle gameInstanceHandle) == false)
                return new GameInstanceReadyResponse();
            
            gameInstanceHandle.SetReady();
            return new GameInstanceReadyResponse();
        }
    }
}