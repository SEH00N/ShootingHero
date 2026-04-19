using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShootingHero.Networks;
using UnityEngine;

namespace ShootingHero.Shared
{
    public class UnityPacketDispatcher : MonoBehaviour, IPacketDispatcher
    {
        private readonly object locker = new object();
        private readonly Queue<(Session, IPacket)> packetQueue = new Queue<(Session, IPacket)>();
        private Lazy<PacketHandlerFactory> packetHandlerFactory = null;

        public void Initialize(IDIContainer diContainer)
        {
            packetHandlerFactory = new Lazy<PacketHandlerFactory>(() => diContainer.GetInstance<PacketHandlerFactory>());
        }

        private void Update()
        {
            if(packetQueue.Count <= 0)
                return;
            
            lock(locker)
            {
                while(packetQueue.TryDequeue(out (Session session, IPacket packet) packetContext))
                {
                    try
                    {
                        Type packetType = packetContext.packet.GetType();
                        Debug.Log($"[UnityPacketDispatcher] Packet Dispatched. PacketType: {packetType.Name}");

                        IPacketHandlerBase packetHandler = packetHandlerFactory?.Value.Create(packetType);
                        if (packetHandler != null)
                            packetHandler.HandlePacket(packetContext.session, packetContext.packet).GetAwaiter().GetResult();
                    }
                    catch(Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                }
            }
        }
        
        public ValueTask Dispatch(Session session, IPacket packet)
        {
            lock(locker)
            {
                packetQueue.Enqueue((session, packet));
                return new ValueTask();
            }
        }
    }
}