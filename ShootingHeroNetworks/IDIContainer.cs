using System;

namespace ShootingHero.Networks
{
    public interface IDIContainer
    {
        TInstance GetInstance<TInstance>() where TInstance : class;
        object GetInstance(Type type);
    }
}