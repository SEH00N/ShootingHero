using System;
using System.Collections.Concurrent;

namespace ShootingHero.LobbyServer
{
    public class GameManager
    {
        private readonly ConcurrentDictionary<string, string> gameList = null;
        private readonly ConcurrentQueue<string> gamePublishQueue = null;

        public GameManager()
        {
            gameList = new ConcurrentDictionary<string, string>();
            gamePublishQueue = new ConcurrentQueue<string>();
        }

        public string PublishGame()
        {
            string uuid;
            
            while(true)
            {
                uuid = Guid.NewGuid().ToString();
                if(gameList.TryAdd(uuid, string.Empty) == true)
                    break;
            }

            gamePublishQueue.Enqueue(uuid);
            return uuid;
        }
    }
}