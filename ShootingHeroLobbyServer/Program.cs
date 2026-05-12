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
            
            builder.Services.AddSingleton<GameManager>();

            builder.Services.AddControllers();
            
            WebApplication app = builder.Build();

            app.MapControllers();
            app.Run();
        }
    }
}

