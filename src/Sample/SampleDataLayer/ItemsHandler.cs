using System.Collections.Generic;
using System.Linq;
using FunnyBus;
using FunnyBus.Infrastructure;
using Ploeh.AutoFixture;
using Sample.Contracts;

namespace Sample.DataLayer
{
    public class ItemsHandler :
        IHandle<LoadItemsMessage, List<SampleItemModel>>,
        IHandle<LoadByIdMessage>
    {
        private readonly IFixture _fixture;

        public ItemsHandler()
        {
            _fixture = new Fixture();
        }

        public List<SampleItemModel> Handle(LoadItemsMessage message)
        {
            return _fixture.CreateMany<SampleItemModel>(10).ToList();
        }

        public void Handle(LoadByIdMessage message)
        {
            //Code code code..
        }
    }
}