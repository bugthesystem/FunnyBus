using System.Collections.Generic;
using System.Linq;
using FunnyBus;
using Sample.BusinessLayer.Models;
using Sample.Contracts;
using Sample.Contracts.Results;

namespace Sample.BusinessLayer
{
    public class HomerControllerAgent : IHomerControllerAgent
    {
        private readonly IBus _bus;

        public HomerControllerAgent(IBus bus)
        {
            _bus = bus;
        }

        public List<SampleItem> GetOrders(GetOrdersModel model)
        {
            var message = new GetOrdersMessage { UserId = model.UserId };

            var result = _bus.Publish<GetOrdersResult>(message);
            return Map(result.Orders);
        }

        private List<SampleItem> Map(IEnumerable<OrderItemModel> publish)
        {
            return publish.Select(model => new SampleItem
            {
                Name = model.Name
            }).ToList();
        }
    }
}