using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ShootingHero.Networks
{
    public class RoomManager : IRoomManager, IPacketDispatcher, IAsyncDisposable, Room.ICallback
    {
        private readonly IPacketDispatcher roomPacketDispatcher = null;
        private readonly ConcurrentDictionary<string, Lazy<Room>> rooms = null;
        private readonly ConcurrentDictionary<Session, Room> sessionRoomMap = null;

        private readonly Lazy<RoomWorker>[] workers = null;
        private readonly Lazy<RoomWorker> dedicatedWorker = null;

        private readonly Lazy<PacketSerializer> packetSerializer = null;
        private readonly Lazy<PacketHandlerFactory> packetHandlerFactory = null;

        public RoomManager(IPacketDispatcher roomPacketDispatcher, DIContainer diContainer, int workerCount, int capacityPerWorker)
        {
            this.roomPacketDispatcher = roomPacketDispatcher;

            rooms = new ConcurrentDictionary<string, Lazy<Room>>();
            sessionRoomMap = new ConcurrentDictionary<Session, Room>();

            packetSerializer = new Lazy<PacketSerializer>(() => diContainer.GetInstance<PacketSerializer>());
            packetHandlerFactory = new Lazy<PacketHandlerFactory>(() => diContainer.GetInstance<PacketHandlerFactory>());

            workers = new Lazy<RoomWorker>[workerCount];
            for(int i = 0; i < workerCount; ++i)
                workers[i] = WorkerFactory(capacityPerWorker);

            dedicatedWorker = WorkerFactory(capacityPerWorker);
        }

        Room IRoomManager.Room(string roomID)
        {
            if(string.IsNullOrEmpty(roomID) == true)
                return null;

            Lazy<Room> room = rooms.GetOrAdd(roomID, RoomFactory);
            return room.Value;
        }

        ValueTask IPacketDispatcher.Dispatch(Session session, IPacket packet)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            try
            {
                if (sessionRoomMap.TryGetValue(session, out Room room) == true)
                {
                    int index = (room.RoomIDHash & int.MaxValue) % workers.Length;
                    return workers[index].Value.EnqueueAsync(session, packet);
                }

                return dedicatedWorker.Value.EnqueueAsync(session, packet);
            }
            catch
            {
                session.Close();
            }
            finally
            {
            }

            return new ValueTask();
        }

        void Room.ICallback.OnAdded(Room room, Session session)
        {
            if (room == null || session == null)
                return;

            sessionRoomMap[session] = room;
        }

        void Room.ICallback.OnRemoved(Room room, Session session)
        {
            if (session == null)
                return;

            sessionRoomMap.TryRemove(session, out _);
        }

        private Lazy<Room> RoomFactory(string roomID)
        {
            return new Lazy<Room>(() => new Room(roomID, packetSerializer.Value, this));
        }

        private Lazy<RoomWorker> WorkerFactory(int capacityPerWorker)
        {
            return new Lazy<RoomWorker>(() => new RoomWorker(roomPacketDispatcher ?? new RoomPacketDispatcher(packetHandlerFactory.Value), capacityPerWorker));
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            foreach (Lazy<RoomWorker> worker in workers)
            {
                if(worker.IsValueCreated == false)
                    continue;

                await (worker.Value as IAsyncDisposable).DisposeAsync();
            }

            if (dedicatedWorker.IsValueCreated == true)
                await (dedicatedWorker.Value as IAsyncDisposable).DisposeAsync();
        }
    }
}
