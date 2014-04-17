using System.Collections.Generic;
using System.Linq;
using FunnyBus;
using FunnyBus.Infrastructure;
using Ploeh.AutoFixture;
using Sample.Contracts;

namespace Sample.DataLayer
{
    public class OrderHandler :
        IHandle<LoadOrdersMessage, List<OrderItemModel>>,
        IHandle<LoadOrderByIdMessage>
    {
        private readonly IBus _bus;
        private readonly IFixture _fixture;

        public OrderHandler(IBus bus)
        {
            _bus = bus;
            _fixture = new Fixture();
        }

        public List<OrderItemModel> Handle(LoadOrdersMessage message)
        {
            _bus.Publish(new DeleteProductMessage { Id = 10 });
            return _fixture.CreateMany<OrderItemModel>(10).ToList();
        }

        public void Handle(LoadOrderByIdMessage message)
        {
            //Code code code..
        }
    }
}