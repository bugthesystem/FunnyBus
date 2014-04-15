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
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(HomeController).Assembly);
            builder.Register(context => Bus.Instance).As<IBus>().SingleInstance();

            builder.RegisterModule<BusinessLayerModule>();

            IContainer container = builder.Build();

            Bus.Configure(context => context.SetResolverAdapter(new AutofacFunnyDependencyResolver(container)));

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}