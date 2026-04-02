using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ShootingHero.Networks
{
    public class RoomWorker : IAsyncDisposable
    {
        private readonly Channel<(Session, IPacket)> channel = null;
        private readonly CancellationTokenSource cancellationTokenSource = null;
        private readonly Task loopTask = null;

        private readonly PacketHandlerFactory packetHandlerFactory = null;

        public RoomWorker(PacketHandlerFactory packetHandlerFactory, int capacity)
        {
            this.packetHandlerFactory = packetHandlerFactory;

            channel = Channel.CreateBounded<(Session, IPacket)>(
                new BoundedChannelOptions(capacity)
                {
                    SingleReader = true,
                    SingleWriter = false,
                    FullMode = BoundedChannelFullMode.Wait,
                    AllowSynchronousContinuations = false
                }
            );

            loopTask = Task.Run(ProcessLoopAsync);
        }

        public ValueTask EnqueueAsync(Session session, IPacket packet)
        {
            return channel.Writer.WriteAsync((session, packet));
        }

        private async Task ProcessLoopAsync()
        {
            try
            {
                while (await channel.Reader.WaitToReadAsync(cancellationTokenSource.Token))
                {
                    while (channel.Reader.TryRead(out (Session session, IPacket packet) packetContext))
                    {
                        try
                        {
                            IPacketHandlerBase packetHandler = packetHandlerFactory.Create(packetContext.packet.GetType());
                            if(packetHandler != null)
                                await packetHandler.HandlePacket(packetContext.session, packetContext.packet);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested) { }
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            channel.Writer.TryComplete();
            cancellationTokenSource.Cancel();

            try
            {
                await loopTask;
            }
            catch (OperationCanceledException) { }
            finally
            {
                cancellationTokenSource.Dispose();
            }
        }
    }
}