using System;
using System.Collections.Generic;
using Autofac;
using FunnyBus;
using FunnyBus.Integration.Autofac;
using Sample.Contracts;
using Sample.DataLayer;

namespace Sample.ConsoleApp
{
    public class Program
    {
        public static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(OrderHandler).Assembly).AsImplementedInterfaces().SingleInstance();
            builder.Register(context => Bus.Instance).As<IBus>();

            IContainer container = builder.Build();
            Bus.Configure(context => context.SetResolverAdapter(new AutofacFunnyDependencyResolver(container)));

            var bus = container.Resolve<IBus>();

            bus.Publish(new CreateProductMessage { Name = "Funny Product" });
            var orders = bus.Publish<LoadOrdersMessage, List<OrderItemModel>>(new LoadOrdersMessage { UserId = 10 });

            Dump(orders);

            int read = Console.Read();
        }

        private static void Dump(IEnumerable<OrderItemModel> homeItemModels)
        {
            foreach (OrderItemModel homeItemModel in homeItemModels)
            {
                Console.WriteLine(homeItemModel.Name);
            }
        }
    }
}
