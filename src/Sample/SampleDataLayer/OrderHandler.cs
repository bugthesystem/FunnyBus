using System;
using FunnyBus;
using FunnyBus.Infrastructure;
using Sample.Contracts;

namespace Sample.DataLayer
{
    public class OrderHandler : IHandle<OrderCreatedMessage>
    {
        private readonly IFunnyBus _bus;

        public OrderHandler(IFunnyBus bus)
        {
            _bus = bus;
        }

        public void Handle(OrderCreatedMessage message)
        {
            Console.WriteLine(message.Message);
            _bus.UnSubscribe(this);//TODO: 
            _bus.Publish(new ProductDeletedMessage { Message = "Called from orderhandler handle Created Message" });
        }
    }
}