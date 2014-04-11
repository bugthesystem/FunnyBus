using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FunnyBus.Infrastructure;
using Ploeh.AutoFixture;
using Sample.Contracts;

namespace Sample.DataLayer
{
    public class OrderHandler :
        IHandle<LoadOrdersMessage, List<OrderItemModel>>,
        IHandle<LoadOrderByIdMessage>
    {
        private readonly IFixture _fixture;

        public OrderHandler()
        {
            _fixture = new Fixture();
        }

        public List<OrderItemModel> Handle(LoadOrdersMessage message)
        {
            return _fixture.CreateMany<OrderItemModel>(10).ToList();
        }

        public void Handle(LoadOrderByIdMessage message)
        {
            Console.WriteLine(message.Id);
            //Code code code..
        }
    }
}