using System;

namespace FunnyBus.Infrastructure.DependencyInjection
{
    public interface IFunnyDependencyScope : IDisposable
    {
        T Resolve<T>();

        T Resolve<T>(string key);

        IFunnyDependencyScope CreateNew();
    }
}