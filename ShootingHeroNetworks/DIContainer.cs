using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShootingHero.Networks
{
    public class DIContainer : IDIContainer
    {
        private readonly Dictionary<Type, object> instances = null;

        public DIContainer()
        {
            instances = new Dictionary<Type, object>();
        }
        
        public void AddInstance<TInstance>(TInstance instance) where TInstance : class
        {
            AddInstance(typeof(TInstance), instance);
        }

        public void AddInstance(Type type, object instance)
        {
            if(type.IsValueType == true)
                throw new InvalidOperationException($"Cannot register an instance of {type.FullName} in the DIContainer because it is not a reference type. Only reference-type instances can be registered.");
            
            instances[type] = instance;
        }

        public TInstance GetInstance<TInstance>() where TInstance : class
        {
            return GetInstance(typeof(TInstance)) as TInstance;
        }

        public object GetInstance(Type type)
        {
            if(instances.TryGetValue(type, out object instance) == true)
                return instance;

            object[] candidates = instances
                .Where(x => type.IsAssignableFrom(x.Key))
                .Select(x => x.Value)
                .ToArray();
            
            if(candidates.Length <= 0)
                return null;

            return candidates[0];
        }

        public async ValueTask DisposeAsync()
        {
            foreach(object instance in instances.Values)
            {
                if(instance is IDisposable disposable)
                    disposable.Dispose();
                else if(instance is IAsyncDisposable asyncDisposable)
                    await asyncDisposable.DisposeAsync();
            }
        }
    }
}