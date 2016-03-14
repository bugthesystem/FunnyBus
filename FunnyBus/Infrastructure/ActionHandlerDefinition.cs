using System;

namespace FunnyBus.Infrastructure
{
    internal class ActionHandlerDefinition : IHandleDefinition
    {
        public Action<object> ProxyAction { get; set; }

        public Type MessageType { get; set; }
    }
}