using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShootingHero.LobbyServer
{
    public class GameInstanceManager
    {
        private readonly ConcurrentDictionary<string, GameInstanceHandle> gameList = null;
        private readonly Queue<string> gamePublishQueue = null;
        private readonly AsyncLock locker = null;


        public GameInstanceManager()
        {
            gameList = new ConcurrentDictionary<string, GameInstanceHandle>();
            gamePublishQueue = new Queue<string>();
            locker = new AsyncLock();
        }

        public async Task<string> PublishGameAsync()
        {
            using (IDisposable lockHandle = await locker.LockAsync())
            {
                string uuid;
                
                while(true)
                {
                    uuid = Guid.NewGuid().ToString();
                    if(gameList.TryAdd(uuid, null) == true)
                        break;
                }

                gamePublishQueue.Enqueue(uuid);
                return uuid;
            }
        }

        public async Task<List<string>> FlushGamePublishQueueAsync()
        {
            using (IDisposable lockHandle = await locker.LockAsync())
            {
                List<string> gamePublishQueueList = new List<string>(gamePublishQueue);
                gamePublishQueue.Clear();

                return gamePublishQueueList;
            }
        }

        public void RegisterGame(string gameUUID, GameInstanceHandle gameInstanceHandle)
        {
            gameList[gameUUID] = gameInstanceHandle;
        }

        public void UnregisterGame(string gameUUID)
        {
            gameList.Remove(gameUUID, out _);
        }

        public async Task StopAllAsync()
        {
            GameInstanceHandle[] gameInstanceHandles = gameList.Values
                .Where(gameInstanceHandle => gameInstanceHandle != null)
                .ToArray();

            gameList.Clear();

            foreach(GameInstanceHandle gameInstanceHandle in gameInstanceHandles)
                await gameInstanceHandle.StopAsync();

            using (IDisposable lockHandle = await locker.LockAsync())
                gamePublishQueue.Clear();
        }

        public bool TryGetGameInstanceHandle(string gameUUID, out GameInstanceHandle gameInstanceHandle)
        {
            return gameList.TryGetValue(gameUUID, out gameInstanceHandle);
        }
    }
}
