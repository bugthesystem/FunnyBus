using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FunnyBus.Exceptions;

namespace FunnyBus.Infrastructure.IoC
{
    internal class PoorDependencyContainer : IPoorDependencyContainer
    {
        private readonly ConcurrentDictionary<Type, Func<object>> _providers
            = new ConcurrentDictionary<Type, Func<object>>();

        public void Bind<TInterfaceType, TClassType>()
            where TClassType : TInterfaceType
        {
            _providers[typeof(TInterfaceType)] = () => ResolveByType(typeof(TClassType));
        }

        public void Bind<TInterfaceType>(object instance)
        {
            _providers[typeof(TInterfaceType)] = () => instance;
        }

        public void Bind<TInterface>(Func<object> factoryMethod)
        {
            _providers[typeof(TInterface)] = factoryMethod;
        }

        public TKey Resolve<TKey>()
        {
            return (TKey)Resolve(typeof(TKey));
        }

        public object Resolve(Type type)
        {
            Func<object> provider;

            if (TryGetValue(type, out provider))
            {
                return provider();
            }
            return ResolveByType(type);
        }

        private bool TryGetValue(Type type, out Func<object> provider)
        {
            var providedKey = _providers.Keys.FirstOrDefault(t => t.GUID.Equals(type.GUID));
            if (providedKey == null)
            {
                provider = null;
                return false;
            }
            provider = _providers[providedKey];
            return true;
        }

        public void TeardownType<TType>()
        {
            Func<object> provider;
            _providers.TryRemove(typeof(TType), out provider);
        }

        public void TeardownContainer()
        {
            _providers.Clear();
        }

        internal object ResolveParameter(Type type)
        {
            //TODO: a stack overflow exception is possible.
            return Resolve(type);
        }

        internal object ResolveByType(Type type)
        {
            var constructor = type.GetConstructors().FirstOrDefault();
            if (typeof(IPoorDependencyContainer).IsAssignableFrom(type))
            {
                return this;
            }
            if (type.IsInterface)
            {
                throw new ResolutionException(type);
            }
            if (constructor == null)
            {
                var instanceProperty = type.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
                if (instanceProperty == null)
                {
                    return Activator.CreateInstance(type);
                }
                return instanceProperty.GetValue(null, null);
            }

            IEnumerable<object> ctorParameters = constructor.GetParameters().Select(p => ResolveParameter(p.ParameterType));

            return constructor.Invoke(ctorParameters.ToArray());
        }
    }
}