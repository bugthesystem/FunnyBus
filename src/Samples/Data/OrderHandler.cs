using System.Linq;
using FunnyBus;
using FunnyBus.Infrastructure;
using Ploeh.AutoFixture;
using Sample.Contracts;
using Sample.Contracts.Results;

namespace Sample.Data
{
    public class OrderHandler :
        IHandle<GetOrdersMessage, GetOrdersResult>,
        IHandle<LoadOrderByIdMessage>
    {
        private readonly IBus _bus;
        private readonly IFixture _fixture;

        public OrderHandler(IBus bus)
        {
            _bus = bus;
            _fixture = new Fixture();
        }

        public GetOrdersResult Handle(GetOrdersMessage message)
        {
            _bus.Publish(new DeleteProductMessage { Id = 10 });
            return new GetOrdersResult {Orders = _fixture.CreateMany<OrderItemModel>(10).ToList()};
        }

        public void Handle(LoadOrderByIdMessage message)
        {
            //Code code code..
        }
    }
}