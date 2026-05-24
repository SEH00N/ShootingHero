using System;
using System.Diagnostics;
using System.Threading;
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
            Process targetProcess = Interlocked.Exchange(ref process, null);
            if (targetProcess == null)
                return;

            try
            {
                if (targetProcess.HasExited == true)
                    return;

                targetProcess.Kill(entireProcessTree: true);
                await targetProcess.WaitForExitAsync();
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
