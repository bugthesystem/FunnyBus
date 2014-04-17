using FunnyBus.Infrastructure.IoC;

namespace FunnyBus.Infrastructure.Configuration
{
    public interface IConfigutaionContext
    {
        void SetResolver(IFunnyDependencyResolver funnyDependencyResolver);
        bool AutoScanHandlers { set; }
    }
}