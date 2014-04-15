using System.Collections.Generic;
using System.Linq;
using FunnyBus;
using Sample.BusinessLayer.Models;
using Sample.Contracts;

namespace Sample.BusinessLayer
{
    public class HomerControllerAgent : IHomerControllerAgent
    {
        private readonly IBus _bus;

        public HomerControllerAgent(IBus bus)
        {
            _bus = bus;
        }

        public List<SampleItem> Get(LoadOrdersModel model)
        {
            var message = new LoadOrdersMessage { UserId = model.UserId };

            var result = _bus.Publish<LoadOrdersMessage,List<OrderItemModel>>(message);
            return Map(result);
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