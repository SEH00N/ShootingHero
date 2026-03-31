using System;
using System.Collections.Generic;

namespace ShootingHero.Networks
{
    public interface INetworkObjectBuilder
    {
        internal DIContainer GetDIContainer();
    }
}