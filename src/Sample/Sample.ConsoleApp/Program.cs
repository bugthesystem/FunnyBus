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

            bus.Publish(new ProductCreatedMessage { Message = "Called from Main (Created Message)" });
            bus.Publish(new ProductDeletedMessage { Message = "Called from Main (Deleted Message)" });

            var homeItemModels = bus.Publish<LoadItemsMessage, List<SampleItemModel>>(new LoadItemsMessage { Prefix = "A.Q" });

            Dump(homeItemModels);

            int read = Console.Read();
        }

        private static void Dump(IEnumerable<SampleItemModel> homeItemModels)
        {
            foreach (SampleItemModel homeItemModel in homeItemModels)
            {
                Console.WriteLine(homeItemModel.Name);
            }
        }
    }
}
