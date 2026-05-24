namespace ShootingHero.Shared
{
    public class LobbyRoomRequest<TRequest, TResponse> : WebRequest<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        public LobbyRoomRequest(TRequest request) : base($"{GameDefine.LOBBY_SERVER_CONNECTION}/Room/{typeof(TRequest).Name}", request) { }
    }
}