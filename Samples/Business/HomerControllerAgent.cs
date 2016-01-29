using System.Collections.Generic;
using System.Linq;
using FunnyBus;
using Sample.Business.Models;
using Sample.Contracts;
using Sample.Contracts.Results;

namespace Sample.Business
{
    public class HomerControllerAgent : IHomerControllerAgent
    {
        private readonly IBus _bus;

        public HomerControllerAgent(IBus bus)
        {
            _bus = bus;
        }

        public List<SampleItem> GetShoppingCart(GetShoppingCartFormModel formModel)
        {
            var message = new GetShoppingCartMessage { UserId = formModel.UserId };

            var result = _bus.Publish<GetShoppingCartResult>(message);
            return Map(result.Orders);
        }

        private List<SampleItem> Map(IEnumerable<CartItem> cartItems)
        {
            return cartItems.Select(model => new SampleItem
            {
                Name = model.Name
            }).ToList();
        }
    }
}