using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ShootingHero.LobbyServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOptions<ServerConfig>()
                .Bind(builder.Configuration.GetSection("ServerConfig"));
            
            builder.Services.AddSingleton<GameInstanceManager>();
            builder.Services.AddSingleton<IPortQueue, PortQueue>();
            builder.Services.AddSingleton<IGameInstanceLauncher, GameInstanceLauncher>();
            builder.Services.AddHostedService<GameScheduleService>();
            builder.Services.AddHostedService<GameInstanceShutdownService>();

            builder.Services.AddSingleton<RoomManager>();

            builder.Services.AddControllers();
            
            WebApplication app = builder.Build();

            app.MapControllers();
            app.Run();
        }
    }
}

