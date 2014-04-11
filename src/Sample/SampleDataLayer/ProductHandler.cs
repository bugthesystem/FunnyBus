using System;
using FunnyBus;
using FunnyBus.Infrastructure;
using Sample.Contracts;

namespace Sample.DataLayer
{
    public class ProductHandler : IHandle<CreateProductMessage>, IHandle<ProductDeletedMessage>
    {
        private readonly IFunnyBus _bus;

        public ProductHandler(IFunnyBus bus)
        {
            _bus = bus;
        }

        public void Handle(CreateProductMessage message)
        {
            _bus.Publish(new LoadOrderByIdMessage { Id = 10 });
            Console.WriteLine(message.Name);
        }

        public void Handle(ProductDeletedMessage message)
        {
            Console.WriteLine(message.Message);
        }
    }
}