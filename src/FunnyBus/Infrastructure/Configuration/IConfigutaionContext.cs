using FunnyBus.Infrastructure.IoC;

namespace FunnyBus.Infrastructure.Configuration
{
    public interface IConfigutaionContext
    {
        void SetResolverAdapter(IFunnyDependencyResolver funnyDependencyResolver);
    }
}