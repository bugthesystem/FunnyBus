using System;
using FunnyBus;
using FunnyBus.Infrastructure;
using Sample.Contracts;

namespace Sample.DataLayer
{
    public class ProductHandler : IHandle<ProductCreatedMessage>, IHandle<ProductDeletedMessage>
    {
        private readonly IFunnyBus _bus;

        public ProductHandler(IFunnyBus bus)
        {
            _bus = bus;
        }

        public void Handle(ProductCreatedMessage message)
        {
            Console.WriteLine(message.Message);
            _bus.Publish(new OrderCreatedMessage { Message = "Called from producthandler handle Created Message" });
        }

        public void Handle(ProductDeletedMessage message)
        {
            Console.WriteLine(message.Message);
            _bus.Publish(new ProductCreatedMessage { Message = "Called from producthandler handle deleted Message" });
        }
    }
}