using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ShootingHero.Networks
{
    public partial class PacketHandlerFactory
    {
        public static class Builder
        {
            public static PacketHandlerFactory Build(Assembly[] assemblies, DIContainer diContainer)
            {
                PacketHandlerFactory packetHandlerFactory = new PacketHandlerFactory() {
                    factories = new Dictionary<Type, Func<DIContainer, IPacketHandlerBase>>()
                };

                Type[] packetHandlerTypes = assemblies
                        .SelectMany(a => a.GetTypes())
                        .Where(t => typeof(IPacketHandlerBase).IsAssignableFrom(t))
                        .Where(t => t.IsDefined(typeof(PacketHandlerAttribute), false))
                        .Where(t => t.IsAbstract == false && t.IsInterface == false)
                        .ToArray();

                foreach (Type packetHandlerType in packetHandlerTypes)
                {
                    PacketHandlerAttribute packetHandlerAttribute = packetHandlerType.GetCustomAttribute<PacketHandlerAttribute>();
                    if(packetHandlerAttribute == null)
                        continue;
                    
                    Type packetType = packetHandlerAttribute.PacketType;
                    if(packetType == null)
                        continue;
                    
                    if(packetType.IsDefined(typeof(PacketAttribute), false) == false || packetType.IsAssignableFrom(typeof(IPacket)) == false)
                        continue;

                    packetHandlerFactory.factories[packetType] = CreatePacketHandlerFactory(packetHandlerType, diContainer);
                }
                
                return packetHandlerFactory;
            }

            private static Func<DIContainer, IPacketHandlerBase> CreatePacketHandlerFactory(Type packetHandlerType, DIContainer diContainer)
            {
                ConstructorInfo constructorInfo = SelectConstructor(packetHandlerType, diContainer);
                if (constructorInfo == null)
                    throw new InvalidOperationException($"No constructor matching the criteria exists for {packetHandlerType.FullName}.");

                ParameterExpression diContainerParam = Expression.Parameter(typeof(DIContainer), "diContainer");
                MethodCallExpression[] constructorArgFillExpressions = constructorInfo
                    .GetParameters()
                    .Select(parameterInfo => {
                        MethodInfo getInstanceMethodInfo = typeof(DIContainer).GetMethod(
                            nameof(DIContainer.GetInstance),
                            BindingFlags.Instance | BindingFlags.Public,
                            binder: null,
                            types: Type.EmptyTypes,
                            modifiers: null
                        );

                        MethodCallExpression getInstanceExpression = Expression.Call(
                            diContainerParam,
                            getInstanceMethodInfo.MakeGenericMethod(parameterInfo.ParameterType)
                        );

                        return getInstanceExpression;
                    })
                    .ToArray();

                NewExpression newExpression = Expression.New(constructorInfo, constructorArgFillExpressions);
                UnaryExpression body = Expression.Convert(newExpression, typeof(IPacketHandlerBase));

                return Expression.Lambda<Func<DIContainer, IPacketHandlerBase>>(body, diContainerParam).Compile();
            }

            private static ConstructorInfo SelectConstructor(Type type, DIContainer diContainer)
            {
                ConstructorInfo[] publicConstructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

                if (publicConstructors.Length == 0)
                    return null;

                ConstructorInfo[] ordered = publicConstructors
                    .OrderByDescending(c => c.GetParameters().Length)
                    .ToArray();

                for (int i = 0; i < ordered.Length; ++i)
                {
                    ConstructorInfo constructorInfo = ordered[i];
                    if (GetIsDependencyInjectableConstructor(constructorInfo, diContainer) == false)
                        continue;

                    return constructorInfo;
                }

                return null;
            }

            private static bool GetIsDependencyInjectableConstructor(ConstructorInfo constructorInfo, DIContainer diContainer)
            {
                ParameterInfo[] parameterInfos = constructorInfo.GetParameters();
                if (parameterInfos == null)
                    return true;

                foreach (ParameterInfo parameterInfo in parameterInfos)
                {
                    if (diContainer.GetInstance(parameterInfo.ParameterType) == null)
                        return false;
                }

                return true;
            }
        }
    }
}