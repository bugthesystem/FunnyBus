using System;
using NUnit.Framework;
using Moq;

using Sample.Contracts;
using Sample.DataLayer;
using FluentAssertions;
using FunnyBus.Exceptions;
using System.Collections.Generic;
using FunnyBus.Infrastructure.Store;

namespace FunnyBus.Tests
{
    public class FunnyBusTests : TestBase
    {
        private Mock<IHandlerStore> _handlersStoreMock;

        private IBus _bus;

        protected override void FinalizeSetUp()
        {
            _handlersStoreMock = MockFor<IHandlerStore>();
            _bus = new Bus(_handlersStoreMock.Object);
        }

        [Test]
        public void Subscribe_Success_Test()
        {
            Type handlerType = typeof(OrderHandler);
            Type messageType = typeof(LoadOrdersMessage);

            _handlersStoreMock.Setup(store => store.Add(handlerType)).Returns(true);
            _handlersStoreMock.Setup(store => store.GetAsIHandleByMessageType(messageType)).Returns(handlerType);

            _bus.Subscribe<OrderHandler>();
            List<OrderItemModel> result = _bus.Publish<LoadOrdersMessage, List<OrderItemModel>>(new LoadOrdersMessage { UserId = 10 });

            result.Should().NotBeEmpty();
        }

        [Test]
        public void Publish_Throws_NotRegisteredException_After_UnSubscribe_Success_Test()
        {
            Type handlerType = typeof(OrderHandler);
            Type messageType = typeof(LoadOrdersMessage);

            _handlersStoreMock.Setup(store => store.Add(handlerType)).Returns(true);
            _handlersStoreMock.Setup(store => store.Remove(handlerType)).Returns(true);
            _handlersStoreMock.Setup(store => store.GetAsIHandleByMessageType(messageType)).Returns((Type)null);

            _bus.Subscribe<OrderHandler>();
            _bus.UnSubscribe<OrderHandler>();

            Assert.Throws<NotRegisteredException>(() => _bus.Publish<LoadOrdersMessage, List<OrderItemModel>>(new LoadOrdersMessage { UserId = 10 }));
        }
    }
}