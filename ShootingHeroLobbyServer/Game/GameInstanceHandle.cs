using System.Diagnostics;
using System.Threading.Tasks;

namespace ShootingHero.LobbyServer
{
    public sealed class GameInstanceHandle
    {
        private Process process = null;
        
        private string host = null;
        public string Host => host;

        private int port = -1;
        public int Port => port;

        private bool isReady = false;
        public bool IsReady => isReady;

        public GameInstanceHandle(Process process, string host, int port)
        {
            this.process = process;
            this.host = host;
            this.port = port;

            isReady = false;
        }

        public void SetReady()
        {
            isReady = true;
        }

        public async Task StopAsync()
        {
            if (process.HasExited)
                return;

            process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync();
        }
    }
}