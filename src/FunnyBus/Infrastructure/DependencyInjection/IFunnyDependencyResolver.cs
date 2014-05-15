using System;
using System.Collections.Generic;

namespace FunnyBus.Infrastructure.DependencyInjection
{
    public interface IFunnyDependencyResolver
    {
        object GetService(Type serviceType);
        IEnumerable<object> GetServices(Type serviceType);
    }
}