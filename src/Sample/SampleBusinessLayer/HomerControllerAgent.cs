using System.Collections.Generic;
using System.Linq;
using FunnyBus;
using Sample.BusinessLayer.Models;
using Sample.Contracts;
using Sample.DataLayer;

namespace Sample.BusinessLayer
{
    public class HomerControllerAgent : IHomerControllerAgent
    {
        private readonly IFunnyBus _bus;

        public HomerControllerAgent(IFunnyBus bus)
        {
            _bus = bus;
        }

        public List<SampleItem> Get(LoadItemsModel model)
        {
            var message = new LoadItemsMessage { Prefix = model.Prefix };

            var result = _bus.Publish<LoadItemsMessage,List<SampleItemModel>>(message);
            return Map(result);
        }

        private List<SampleItem> Map(IEnumerable<SampleItemModel> publish)
        {
            return publish.Select(model => new SampleItem
            {
                Name = model.Name
            }).ToList();
        }
    }
}