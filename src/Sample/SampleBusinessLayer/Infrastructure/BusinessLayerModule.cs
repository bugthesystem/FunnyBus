using Autofac;
using Sample.DataLayer;

namespace Sample.BusinessLayer.Infrastructure
{
    public class BusinessLayerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ItemsHandler).Assembly).AsSelf().SingleInstance(); //TODO: As Implemented Interfaces
            builder.RegisterType<HomerControllerAgent>().As<IHomerControllerAgent>().SingleInstance();
            base.Load(builder);
        }
    }
}