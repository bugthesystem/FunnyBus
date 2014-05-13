using FunnyBus.Infrastructure.IoC;

namespace FunnyBus.Infrastructure.Configuration
{
    public interface IConfigurationContext
    {
        void SetResolver(IFunnyDependencyResolver funnyDependencyResolver);
        bool AutoScanHandlers { set; }
    }
}