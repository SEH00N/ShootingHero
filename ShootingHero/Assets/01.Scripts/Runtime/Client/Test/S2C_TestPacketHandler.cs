using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_TestPacket))]
    public class S2C_TestPacketHandler : IPacketHandler<S2C_TestPacket>
    {
        private Test test = null;

        public S2C_TestPacketHandler(Test test)
        {
            this.test = test;
        }

        ValueTask IPacketHandler<S2C_TestPacket>.HandlePacket(Session session, S2C_TestPacket packet)
        {
            test.TextField.text += $"\n{packet.Message}";
            return new ValueTask();
        }
    }
}