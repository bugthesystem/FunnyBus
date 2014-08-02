using System;
using Autofac;
using FunnyBus;
using Sample.Data;
using Sample.Contracts;
using Sample.Contracts.Results;
using System.Collections.Generic;
using FunnyBus.Integration.Autofac;

namespace Sample.ConsoleApp
{
    public class Program
    {
        public static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(ShoppingCartHandler).Assembly).AsImplementedInterfaces().SingleInstance();
            builder.Register(context => Bus.Instance).As<IBus>();

            IContainer container = builder.Build();
            Bus.Configure(context => context.SetResolver(new AutofacFunnyDependencyResolver(container)));

            var bus = container.Resolve<IBus>();

            bus.Publish(new CreateProductMessage { Name = "Funny Product" });
            //Publish Impl 1
            GetShoppingCartResult shoppingCartResult = bus.Publish<GetShoppingCartMessage, GetShoppingCartResult>(new GetShoppingCartMessage { UserId = 10 });

            //Publish Impl 2
            GetShoppingCartResult result = bus.Publish<GetShoppingCartResult>(new GetShoppingCartMessage { UserId = 10 });

            Dump(shoppingCartResult.Orders);
            Dump(result.Orders);

            bus.Subscribe<OperationCompletedMessage>(message => Console.WriteLine(message.Result));
            bus.Publish(new OperationCompletedMessage { Result = "Operation completed." });

            Console.Read();
        }

        private static void Dump(IEnumerable<CartItem> homeItemModels)
        {
            foreach (CartItem homeItemModel in homeItemModels)
            {
                Console.WriteLine(homeItemModel.Name);
            }
        }
    }
}
