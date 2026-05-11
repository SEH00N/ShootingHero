using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShootingHero.LobbyServer
{
    [ApiController]
    [Route("Room")]
    public class RoomController : ControllerBase
    {
        public RoomController()
        {
            
        }

        [HttpPost(nameof(CreateRoomRequest))]
        public async Task<ActionResult<CreateRoomResponse>> CreateRoomPost([FromBody]CreateRoomRequest request)
        {
            return new CreateRoomResponse();
        }
    }
}