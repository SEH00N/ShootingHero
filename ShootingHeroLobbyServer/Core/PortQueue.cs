using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ShootingHero.LobbyServer
{
    public interface IPortQueue
    {
        Task<int> RentAsync();
        void Release(int port);
    }

    public class PortQueue : IPortQueue
    {
        private readonly ConcurrentQueue<int> availablePorts = new();
        private readonly SemaphoreSlim semaphore;

        public PortQueue(IOptions<ServerConfig> serverConfigOptions)
        {
            ServerConfig serverConfig = serverConfigOptions.Value;
            for (int port = serverConfig.PortQueueRangeMin; port <= serverConfig.PortQueueRangeMax; port++)
                availablePorts.Enqueue(port);

            semaphore = new SemaphoreSlim(availablePorts.Count);
        }

        public async Task<int> RentAsync()
        {
            await semaphore.WaitAsync();

            if (availablePorts.TryDequeue(out int port))
                return port;

            semaphore.Release();
            return -1;
        }

        public void Release(int port)
        {
            availablePorts.Enqueue(port);
            semaphore.Release();
        }
    }
}