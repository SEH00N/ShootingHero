namespace ShootingHero.Shared
{
    public class LobbyGameRequest<TRequest, TResponse> : WebRequest<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        public LobbyGameRequest(TRequest request) : base($"{GameDefine.LOBBY_SERVER_CONNECTION}/Game/{typeof(TRequest).Name}", request) { }
    }
}