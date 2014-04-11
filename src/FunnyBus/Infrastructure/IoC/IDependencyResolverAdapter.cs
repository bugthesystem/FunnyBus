using System;
using System.Collections.Generic;

namespace FunnyBus.Infrastructure.IoC
{
    public interface IDependencyResolverAdapter
    {
        object GetService(Type serviceType);
        IEnumerable<object> GetServices(Type serviceType);
    }
}