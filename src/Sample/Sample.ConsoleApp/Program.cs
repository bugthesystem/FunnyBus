using System;
using System.Collections.Generic;
using Autofac;
using FunnyBus;
using FunnyBus.Integration.Autofac;
using Sample.Contracts;
using Sample.Contracts.Results;
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
            Bus.Configure(context => context.SetResolver(new AutofacFunnyDependencyResolver(container)));

            var bus = container.Resolve<IBus>();

            bus.Publish(new CreateProductMessage { Name = "Funny Product" });
            //Publish Impl 1
            var orders = bus.Publish<GetOrdersMessage, List<OrderItemModel>>(new GetOrdersMessage { UserId = 10 });
            
            //Publism Impl 2
            GetOrdersResult result = bus.Publish<GetOrdersResult>(new GetOrdersMessage { UserId = 10 });

            Dump(orders);
            Dump(result.Orders);

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
