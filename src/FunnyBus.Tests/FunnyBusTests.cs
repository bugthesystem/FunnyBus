using Moq;
using System;
using NUnit.Framework;
using FluentAssertions;
using FunnyBus.Tests.App;
using FunnyBus.Exceptions;
using FunnyBus.Infrastructure;
using System.Collections.Generic;
using FunnyBus.Infrastructure.Store;
using FunnyBus.Infrastructure.DependencyInjection;

namespace FunnyBus.Tests
{
    public class FunnyBusTests : TestBase
    {
        private Mock<IHandlerStore> _handlersStoreMock;
        private Mock<IFunnyDependencyResolver> _dependencyResolverMock;

        private IBus _bus;

        protected override void FinalizeSetUp()
        {
            _handlersStoreMock = MockFor<IHandlerStore>();
            _dependencyResolverMock = MockFor<IFunnyDependencyResolver>();
            _bus = new Bus(_handlersStoreMock.Object) { DependencyResolver = _dependencyResolverMock.Object, AutoScanHandlers = false };
        }

        [Test]
        public void Publish_With_Message_And_ReturnType_Test()
        {
            var handler = new TestHandler();
            Type handlerType = handler.GetType();
            Type messageType = new TestMessage().GetType();

            _handlersStoreMock.Setup(store => store.Add(handlerType)).Returns(true);
            _handlersStoreMock.Setup(store => store.GetAsIHandleByMessageType(messageType)).Returns(handlerType);
            _dependencyResolverMock.Setup(resolver => resolver.GetService(handlerType)).Returns(handler);

            _bus.Subscribe<TestHandler>();
            TestMessageResult result = _bus.Publish<TestMessage, TestMessageResult>(new TestMessage());

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Publish_With_ReturnType_Test()
        {
            var handler = new TestHandler();
            Type handlerType = handler.GetType();
            Type messageType = new TestMessage().GetType();

            _handlersStoreMock.Setup(store => store.Add(handlerType)).Returns(true);
            _handlersStoreMock.Setup(store => store.GetAsIHandleByMessageType(messageType)).Returns(handlerType);
            _dependencyResolverMock.Setup(resolver => resolver.GetService(handlerType)).Returns(handler);

            _bus.Subscribe<TestHandler>();
            TestMessageResult result = _bus.Publish<TestMessageResult>(new TestMessage());

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Publish_Throws_HandlerNotFoundException_After_UnSubscribe_Test()
        {
            var handler = new TestHandler();
            Type handlerType = handler.GetType();
            TestMessage testMessage = new TestMessage();
            Type messageType = testMessage.GetType();

            _handlersStoreMock.Setup(store => store.Add(handlerType)).Returns(true);
            _handlersStoreMock.Setup(store => store.Remove(handlerType)).Returns(true);
            _handlersStoreMock.Setup(store => store.GetAsIHandleByMessageType(messageType)).Returns((Type)null);

            _bus.Subscribe<TestHandler>();
            _bus.UnSubscribe<TestHandler>();

            Assert.Throws<HandlerNotFoundException>(() => _bus.Publish<TestMessage, TestMessageResult>(testMessage));
        }

        [Test]
        public void Subscribe_Action_For_Message_Type_Test()
        {
            TestMessage testMessage = new TestMessage();
            Type messageType = testMessage.GetType();
            Action<object> proxyAction = o => { };
            List<ActionHandlerDefinition> handlerDefinitions = new List<ActionHandlerDefinition>
            {
                new ActionHandlerDefinition
                {
                    ProxyAction = proxyAction, 
                    MessageType = messageType
                }
            };

            _handlersStoreMock.Setup(store => store.AddActionHandler(messageType, It.IsAny<Action<object>>())).Returns(true);
            _handlersStoreMock.Setup(store => store.IsActionHandler(messageType)).Returns(true);
            _handlersStoreMock.Setup(store => store.GetActionHandlerDefinitionsByMessageType(messageType)).Returns(handlerDefinitions);

            _bus.Subscribe<TestMessage>(message => { });
            _bus.Publish(new TestMessage());
        }
    }
}