using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShootingHero.LobbyServer
{
    [ApiController]
    [Route("Room")]
    public class RoomController : ControllerBase
    {
        private readonly RoomManager roomManager = null;

        public RoomController(RoomManager roomManager)
        {
            this.roomManager = roomManager;
        }

        [HttpPost(nameof(CreateRoomRequest))]
        public async Task<ActionResult<CreateRoomResponse>> CreateRoomPost([FromBody]CreateRoomRequest request)
        {
            string roomUUID = roomManager.CreateRoom();
            if(roomManager.TryGetRoom(roomUUID, out Room room) == false)
                return new CreateRoomResponse() { RoomUUID = null };
            
            using(IDisposable lockHandle = await room.LockAsync())
            {
                room.Create(request.Nickname);
                return new CreateRoomResponse() { RoomUUID = roomUUID };
            }
        }

        [HttpPost(nameof(JoinRoomRequest))]
        public async Task<ActionResult<JoinRoomResponse>> JoinRoomPost([FromBody]JoinRoomRequest request)
        {
            if(string.IsNullOrEmpty(request.RoomUUID) == true)
                return new JoinRoomResponse() { Result = false };

            if(roomManager.TryGetRoom(request.RoomUUID, out Room room) == false)
                return new JoinRoomResponse() { Result = false };

            using(IDisposable lockHandle = await room.LockAsync())
            {
                if(room.TryJoin(request.Nickname) == false)
                    return new JoinRoomResponse() { Result = false };
                
                return new JoinRoomResponse() { Result = true };
            }
        }

        [HttpPost(nameof(ExitRoomRequest))]
        public async Task<ActionResult<ExitRoomResponse>> ExitRoomPost([FromBody]ExitRoomRequest request)
        {
            if(string.IsNullOrEmpty(request.RoomUUID) == true)
                return new ExitRoomResponse();

            if(roomManager.TryGetRoom(request.RoomUUID, out Room room) == false)
                return new ExitRoomResponse();

            using(IDisposable lockHandle = await room.LockAsync())
            {
                room.Exit(request.Nickname);
                return new ExitRoomResponse();
            }
        }

        [HttpPost(nameof(ReadyRoomRequest))]
        public async Task<ActionResult<ReadyRoomResponse>> ReadyRoomPost([FromBody]ReadyRoomRequest request)
        {
            if(string.IsNullOrEmpty(request.RoomUUID) == true)
                return new ReadyRoomResponse();

            if(roomManager.TryGetRoom(request.RoomUUID, out Room room) == false)
                return new ReadyRoomResponse();

            using(IDisposable lockHandle = await room.LockAsync())
            {
                room.Ready(request.Nickname);
                return new ReadyRoomResponse();
            }
        }

        [HttpPost(nameof(StartRoomRequest))]
        public async Task<ActionResult<StartRoomResponse>> StartRoomPost([FromBody]StartRoomRequest request)
        {
            if(string.IsNullOrEmpty(request.RoomUUID) == true)
                return new StartRoomResponse();

            if(roomManager.TryGetRoom(request.RoomUUID, out Room room) == false)
                return new StartRoomResponse();

            using(IDisposable lockHandle = await room.LockAsync())
            {
                await room.StartAsync();
                return new StartRoomResponse();
            }
        }

        [HttpPost(nameof(UpdateRoomCommandRequest))]
        public async Task<ActionResult<UpdateRoomCommandResponse>> UpdateRoomCommandPost([FromBody]UpdateRoomCommandRequest request)
        {
            if(string.IsNullOrEmpty(request.RoomUUID) == true)
                return new UpdateRoomCommandResponse();

            if(roomManager.TryGetRoom(request.RoomUUID, out Room room) == false)
                return new UpdateRoomCommandResponse();

            TimeSpan timeout = TimeSpan.FromSeconds(30);
            DateTime expireAt = DateTime.UtcNow + timeout;

            int targetIndex = request.Cursor + 1;
            while(DateTime.UtcNow < expireAt)
            {
                int diff = room.GetRoomCommandCount() - targetIndex;
                if(diff <= 0)
                {
                    await Task.Delay(500);
                    continue;
                }

                List<RoomCommand> roomCommands = room.GetRoomCommandsRange(targetIndex, diff);
                return new UpdateRoomCommandResponse() { RoomCommands = roomCommands };
            }

            return new UpdateRoomCommandResponse();
        }
    }
}