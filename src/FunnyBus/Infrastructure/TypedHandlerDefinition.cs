using System;

namespace FunnyBus.Infrastructure
{
    internal class TypedHandlerDefinition : IHandleDefinition
    {
        public string MessageFullName { get; set; }
        public Type MessageType { get; set; }
        public bool HasReturnType { get; set; }
        public Type ReturnType { get; set; }
    }
}