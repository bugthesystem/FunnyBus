using System;
using Sample.Contracts;
using FunnyBus.Infrastructure;

namespace Sample.Data
{
    public class ProductHandler : IHandle<CreateProductMessage>, IHandle<DeleteProductMessage>
    {
        public void Handle(CreateProductMessage message)
        {
            Console.WriteLine(message.Name);
        }

        public void Handle(DeleteProductMessage message)
        {
            Console.WriteLine(message.Id);
        }
    }
}