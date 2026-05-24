using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ShootingHero.LobbyServer
{
    public interface IGameInstanceLauncher
    {
        Task<GameInstanceHandle> LaunchAsync(string uuid, int port);
    }

    public class GameInstanceLauncher : IGameInstanceLauncher
    {
        private readonly ServerConfig serverConfig = null;

        public GameInstanceLauncher(IOptions<ServerConfig> serverConfigOptions)
        {
            serverConfig = serverConfigOptions.Value;;
        }

        public async Task<GameInstanceHandle> LaunchAsync(string uuid, int port)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = serverConfig.ExecutablePath,
                Arguments = $"--uuid={uuid} --port={port}",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = Process.Start(startInfo);

            await Task.Delay(100);

            if (process == null)
                throw new InvalidOperationException("Failed to start game server process.");

            return new GameInstanceHandle(process, serverConfig.Host, port);
        }
    }
}