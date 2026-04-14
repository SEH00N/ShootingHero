using System.Threading.Tasks;

namespace ShootingHero.Networks
{
    public class RoomPacketDispatcher : IPacketDispatcher
    {
        private PacketHandlerFactory packetHandlerFactory = null;
        
        public RoomPacketDispatcher(PacketHandlerFactory packetHandlerFactory)
        {
            this.packetHandlerFactory = packetHandlerFactory;
        }

        public ValueTask Dispatch(Session session, IPacket packet)
        {
            IPacketHandlerBase packetHandler = packetHandlerFactory.Create(packet.GetType());
            if (packetHandler != null)
                return packetHandler.HandlePacket(session, packet);
            
            return new ValueTask();
        }
    }
}