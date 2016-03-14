using System;
using FunnyBus.Infrastructure;
using Sample.Contracts;

namespace Sample.Data
{
    public class AnotherProductHandler : IHandle<CreateProductMessage>
    {
        public void Handle(CreateProductMessage message)
        {
            Console.WriteLine(String.Join(" ", "AnotherProductHandler", message.Name));
        }
    }
}