using System;

namespace ShootingHero.Networks
{
    public interface IDIContainer : IAsyncDisposable
    {
        TInstance GetInstance<TInstance>() where TInstance : class;
        object GetInstance(Type type);
    }
}