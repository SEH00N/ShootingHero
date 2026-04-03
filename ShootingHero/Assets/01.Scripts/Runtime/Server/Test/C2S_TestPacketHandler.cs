using System;
using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Servers
{
    [PacketHandler(typeof(C2S_TestPacket))]
    public class C2S_TestPacketHandler : IPacketHandler<C2S_TestPacket>
    {
        private Test test = null;
        private Server server = null;

        public C2S_TestPacketHandler(Test test, Server server)
        {
            this.test = test;
            this.server = server;
        }

        ValueTask IPacketHandler<C2S_TestPacket>.HandlePacket(Session session, C2S_TestPacket packet)
        {
            Room room = server.Rooms.Room("test");

            if (test.SessionMap.TryGetValue(session, out string sessionID) == false)
            {
                sessionID = Guid.NewGuid().ToString();
                room.Add(sessionID, session);
                test.SessionMap[session] = sessionID;
            }

            S2C_TestPacket broadcastPacket = new S2C_TestPacket() { 
                Message = $"{sessionID}: {packet.Message}"
            };

            room.Send(broadcastPacket);

            return new ValueTask();
        }
    }
}