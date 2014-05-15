using Autofac;
using Sample.Data;

namespace Sample.Business.Infrastructure
{
    public class BusinessLayerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(OrderHandler).Assembly).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<HomerControllerAgent>().As<IHomerControllerAgent>().SingleInstance();
            base.Load(builder);
        }
    }
}