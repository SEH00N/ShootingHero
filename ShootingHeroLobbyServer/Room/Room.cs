using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShootingHero.LobbyServer
{
    public class Room
    {
        public interface ICallback
        {
            public Task<string> StartGameAsync();
        }

        private readonly Dictionary<string, Member> memberList = null;
        private readonly List<RoomCommand> roomCommands = null;
        private readonly AsyncLock locker = null;
        private readonly ICallback callback = null;

        private string roomOwnerNickname = "";
        
        public Room(ICallback callback)
        {
            this.callback = callback;

            memberList = new Dictionary<string, Member>();
            roomCommands = new List<RoomCommand>();
            locker = new AsyncLock();
            
            roomOwnerNickname = "";
        }

        public Task<IDisposable> LockAsync()
        {
            return locker.LockAsync();
        }

        public void Create(string nickname)
        {
            roomOwnerNickname = nickname;
            memberList[nickname] = new Member();
            AddRoomCommand(ERoomCommandType.Create, nickname);
        }

        public bool TryJoin(string nickname)
        {
            if(memberList.ContainsKey(nickname) == true)
                return false;
            
            memberList[nickname] = new Member();
            AddRoomCommand(ERoomCommandType.Join, nickname);
            return true;
        }

        public void Exit(string nickname)
        {
            memberList.Remove(nickname);
            AddRoomCommand(ERoomCommandType.Exit, nickname);

            if(roomOwnerNickname == nickname)
                AddRoomCommand(ERoomCommandType.Close, null);
        }

        public void Ready(string nickname)
        {
            if(memberList.TryGetValue(nickname, out Member member) == false)
                return;
            
            if(member.IsReady == true)
                return;
            
            member.IsReady = true;
            AddRoomCommand(ERoomCommandType.Ready, nickname);
        }

        public async Task StartAsync()
        {
            foreach(string nickname in memberList.Keys)
            {
                if(nickname == roomOwnerNickname)
                    continue;
                
                if(memberList[nickname].IsReady == false)
                    return;
            }

            foreach(Member member in memberList.Values)
                member.IsReady = false;
            
            string gameUUID = await callback.StartGameAsync();
            AddRoomCommand(ERoomCommandType.Start, gameUUID);
        }

        public int GetRoomCommandCount()
        {
            return roomCommands.Count;
        }

        public List<RoomCommand> GetRoomCommandsRange(int from, int count)
        {
            return roomCommands.GetRange(from, count);
        }

        private void AddRoomCommand(ERoomCommandType commandType, string commandData)
        {
            roomCommands.Add(new RoomCommand() {
                RoomCommandType = commandType,
                RoomCommandData = commandData, 
            });
        }
    }
}