using System;
using System.Collections.Generic;
using FluentAssertions;
using FunnyBus.Exceptions;
using FunnyBus.Infrastructure.Store;
using Moq;
using NUnit.Framework;
using Sample.Contracts;
using Sample.DataLayer;

namespace FunnyBus.Tests
{
    public class FunnyBusTests : TestBase
    {
        private Mock<IHandlerStore> _handlersStoreMock;

        private IFunnyBus _bus;

        protected override void FinalizeSetUp()
        {
            _handlersStoreMock = MockFor<IHandlerStore>();
            _bus = new FunnyBus(_handlersStoreMock.Object);
        }

        [Test]
        public void Subscribe_Success_Test()
        {
            Type handlerType = typeof(ItemsHandler);
            Type messageType = typeof(LoadItemsMessage);

            _handlersStoreMock.Setup(store => store.Add(handlerType)).Returns(true);
            _handlersStoreMock.Setup(store => store.CheckByMessageType(messageType)).Returns(handlerType);

            _bus.Subscribe<ItemsHandler>();
            List<SampleItemModel> result = _bus.Publish<LoadItemsMessage, List<SampleItemModel>>(new LoadItemsMessage { Prefix = "A.Q" });

            result.Should().NotBeEmpty();
        }

        [Test]
        public void Publish_Throws_NotRegiteredException_After_UnSubscribe_Success_Test()
        {
            Type handlerType = typeof(ItemsHandler);
            Type messageType = typeof(LoadItemsMessage);

            _handlersStoreMock.Setup(store => store.Add(handlerType)).Returns(true);
            _handlersStoreMock.Setup(store => store.Remove(handlerType)).Returns(true);
            _handlersStoreMock.Setup(store => store.CheckByMessageType(messageType)).Returns((Type)null);

            _bus.Subscribe<ItemsHandler>();
            _bus.UnSubscribe<ItemsHandler>();

            Assert.Throws<NotRegisteredException>(() => _bus.Publish<LoadItemsMessage, List<SampleItemModel>>(new LoadItemsMessage { Prefix = "A.Q" }));
        }
    }
}