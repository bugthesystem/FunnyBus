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
            Console.WriteLine(message.Name);
            _bus.Publish(new OrderCreatedMessage { Message = "Called from producthandler handle Created Name" });
        }

        public void Handle(ProductDeletedMessage message)
        {
            Console.WriteLine(message.Message);
            _bus.Publish(new CreateProductMessage { Name = "Called from producthandler handle deleted Name" });
        }
    }
}