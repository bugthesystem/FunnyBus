using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using FunnyBus;
using FunnyBus.Integration.Autofac;
using Sample.BusinessLayer.Infrastructure;
using Sample.MvcApp.Controllers;

namespace Sample.MvcApp
{
    public class IoCConfig
    {
        public static void Register()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(HomeController).Assembly);
            builder.Register(context => FunnyBus.FunnyBus.Instance).As<IFunnyBus>().SingleInstance();

            builder.RegisterModule<BusinessLayerModule>();

            IContainer container = builder.Build();

            FunnyBus.FunnyBus.Configure(context => context.SetResolverAdapter(new AutofacDependencyResolverAdapter(container)));

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}