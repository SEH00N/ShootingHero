using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShootingHero.LobbyServer
{
    public class Room
    {
        private readonly Dictionary<string, Member> memberList = null;
        private readonly List<RoomCommand> roomCommands = null;
        private readonly AsyncLock locker = null;
        
        private int commandCounter = 0;
        private string roomOwnerNickname = "";

        private readonly GameManager gameManager = null;
        
        public Room(GameManager gameManager)
        {
            this.gameManager = gameManager;

            memberList = new Dictionary<string, Member>();
            roomCommands = new List<RoomCommand>();
            locker = new AsyncLock();
            
            commandCounter = 0;
            roomOwnerNickname = "";
        }

        public Task<IDisposable> LockAsync()
        {
            return locker.LockAsync();
        }

        public void Create()
        {
            AddRoomCommand(ERoomCommandType.Create, null);
        }

        public void Close()
        {
            AddRoomCommand(ERoomCommandType.Exit, null);
        }

        public bool TryJoin(string nickname)
        {
            if(memberList.ContainsKey(nickname) == true)
                return false;
            
            memberList.Add(nickname, new Member() {
                Nickname = nickname
            });

            if(string.IsNullOrEmpty(roomOwnerNickname) == true)
                roomOwnerNickname = nickname;
        
            AddRoomCommand(ERoomCommandType.Join, nickname);
            return true;
        }

        public void Exit(string nickname)
        {
            memberList.Remove(nickname);
            AddRoomCommand(ERoomCommandType.Exit, nickname);
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

        public void Start()
        {
            foreach(Member member in memberList.Values)
            {
                if(member.Nickname == roomOwnerNickname)
                    continue;
                
                if(member.IsReady == false)
                    break;
            }

            foreach(Member member in memberList.Values)
                member.IsReady = false;
            
            string gameUUID = gameManager.PublishGame();
            AddRoomCommand(ERoomCommandType.Start, gameUUID);
        }

        private void AddRoomCommand(ERoomCommandType commandType, string commandData)
        {
            int index = commandCounter++;
            roomCommands.Add(new RoomCommand() {
                Index = index,
                RoomCommandType = commandType,
                RoomCommandData = commandData, 
            });
        }
    }
}