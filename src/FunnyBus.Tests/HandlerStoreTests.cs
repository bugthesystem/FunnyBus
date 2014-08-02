using System;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using FunnyBus.Tests.App;
using FunnyBus.Infrastructure;
using System.Collections.Generic;
using FunnyBus.Infrastructure.Store;

namespace FunnyBus.Tests
{
    public class HandlerStoreTests : TestBase
    {
        private IHandlerStore _handlerStore;
        private TestHandler _handler;
        private Type _handlerType;
        private TestMessage _message;
        private Type _messageType;

        protected override void FinalizeSetUp()
        {
            _handlerStore = new HandlerStore();

            _handler = new TestHandler();
            _handlerType = _handler.GetType();

            _message = new TestMessage();
            _messageType = _message.GetType();
        }

        [Test]
        public void Add_Test()
        {
            bool addResult = _handlerStore.Add(_handlerType);

            addResult.Should().BeTrue();
            Type handlerResult = _handlerStore.Get(_handlerType);

            handlerResult.Should().NotBeNull();
            handlerResult.Should().Be<TestHandler>();
        }

        [Test]
        public void GetByMessageType_Test()
        {
            bool addResult = _handlerStore.Add(_handlerType);

            addResult.Should().BeTrue();
            Type handlerResult = _handlerStore.GetByMessageType(_messageType);

            handlerResult.Should().NotBeNull();
            handlerResult.Should().Be<TestHandler>();
        }

        [Test]
        public void GetAsIHandleByMessageType_Test()
        {
            bool addResult = _handlerStore.Add(_handlerType);

            addResult.Should().BeTrue();
            Type handlerResult = _handlerStore.GetAsIHandleByMessageType(_messageType);

            handlerResult.Should().NotBeNull();
            handlerResult.Should().Be<IHandle<TestMessage, TestMessageResult>>();
        }

        [Test]
        public void Remove_Test()
        {
            bool addResult = _handlerStore.Add(_handlerType);

            addResult.Should().BeTrue();
            bool removeResult = _handlerStore.Remove(_handlerType);

            removeResult.Should().BeTrue();

            Type handlerResult = _handlerStore.Get(_handlerType);

            handlerResult.Should().BeNull();
        }

        [Test]
        public void AddActionHandler_Test()
        {
            Action<object> proxyAction = o => { };
            bool addResult = _handlerStore.AddActionHandler(_messageType, proxyAction);

            addResult.Should().BeTrue();

            List<ActionHandlerDefinition> handlerDefinitions = _handlerStore.GetActionHandlerDefinitionsByMessageType(_messageType);

            handlerDefinitions.Should().NotBeEmpty();
            handlerDefinitions.Count.Should().Be(1);
            handlerDefinitions.First().ProxyAction.Should().Be(proxyAction);
        }

        [Test]
        public void IsActionHandler_Returns_True_When_Handler_Is_Action_Test()
        {
            Action<object> proxyAction = o => { };
            bool addResult = _handlerStore.AddActionHandler(_messageType, proxyAction);

            addResult.Should().BeTrue();

            bool result = _handlerStore.IsActionHandler(_messageType);

            result.Should().BeTrue();
        }

        [Test]
        public void IsActionHandler_Returns_False_When_Handler_Is_Not_Action_Test()
        {
            bool addResult = _handlerStore.Add(_handlerType);

            addResult.Should().BeTrue();

            bool result = _handlerStore.IsActionHandler(_messageType);

            result.Should().BeFalse();
        }
    }
}
