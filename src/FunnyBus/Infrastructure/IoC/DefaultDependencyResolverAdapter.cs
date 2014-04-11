using System;
using System.Collections.Generic;

namespace FunnyBus.Infrastructure.IoC
{
    internal class DefaultDependencyResolverAdapter : IDependencyResolverAdapter
    {
        private readonly IPoorDependencyContainer _container;

        public DefaultDependencyResolverAdapter(IPoorDependencyContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            throw new NotImplementedException("Not Implemented");
        }
    }
}