using FunnyBus;
using System.Linq;
using Sample.Contracts;
using Ploeh.AutoFixture;
using FunnyBus.Infrastructure;
using Sample.Contracts.Results;

namespace Sample.Data
{
    public class ShoppingCartHandler :
        IHandle<GetShoppingCartMessage, GetShoppingCartResult>,
        IHandle<GetCartItemByIdMessage>
    {
        private readonly IBus _bus;
        private readonly IFixture _fixture;

        public ShoppingCartHandler(IBus bus)
        {
            _bus = bus;
            _fixture = new Fixture();
        }

        public GetShoppingCartResult Handle(GetShoppingCartMessage message)
        {
            _bus.Publish(new DeleteProductMessage { Id = 10 });
            return new GetShoppingCartResult { Orders = _fixture.CreateMany<CartItem>(10).ToList() };
        }

        public void Handle(GetCartItemByIdMessage message)
        {
            //Code..
        }
    }
}