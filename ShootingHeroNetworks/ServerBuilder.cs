using System;

namespace ShootingHero.Networks
{
    public class ServerBuilder : NetworkObjectBuilder<Server>
    {
        private int workerCount = -1;
        private int capacityPerWorker = 4096;

        private IPacketDispatcher roomPacketDispatcher = null;

        public ServerBuilder(ISessionFactory sessionFactory, IPacketDispatcher roomPacketDispatcher = null) : base()
        {
            this.roomPacketDispatcher = roomPacketDispatcher;
            AddSingleton<ISessionFactory>(sessionFactory);
        }

        public ServerBuilder SetWorkerCount(int workerCount)
        {
            this.workerCount = workerCount;
            return this;
        }

        public ServerBuilder SetCapacityPerWorker(int capacityPerWorker)
        {
            this.capacityPerWorker = capacityPerWorker;
            return this;
        }

        protected override Server OnBuild()
        {
            if(workerCount <= 0)
                workerCount = Environment.ProcessorCount;

            RoomManager roomManager = new RoomManager(roomPacketDispatcher, diContainer, workerCount, capacityPerWorker);
            AddSingleton<IPacketDispatcher>(roomManager);
            AddSingleton<IRoomManager>(roomManager);

            return new Server(this);
        }
    }
}