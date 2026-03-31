using System;

namespace ShootingHero.Networks
{
    public interface ISendQueueContext : IDisposable
    {
        ArraySegment<byte> GetData();
    }
}