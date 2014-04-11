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
            builder.RegisterAssemblyTypes(typeof(OrderHandler).Assembly).AsSelf().SingleInstance(); //TODO: As Implemented Interfaces
            builder.Register(context => FunnyBus.FunnyBus.Instance).As<IFunnyBus>();

            IContainer container = builder.Build();
            FunnyBus.FunnyBus.Configure(context => context.SetResolverAdapter(new AutofacDependencyResolverAdapter(container)));

            var bus = container.Resolve<IFunnyBus>();

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
