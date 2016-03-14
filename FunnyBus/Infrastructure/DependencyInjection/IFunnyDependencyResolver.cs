using System;
using System.Collections.Generic;

namespace FunnyBus.Infrastructure.DependencyInjection
{
    public interface IFunnyDependencyResolver : IDisposable
    {
        object GetService(Type serviceType);

        IEnumerable<object> GetServices(Type serviceType);

        IFunnyDependencyResolver BeginNewScope();
    }
}