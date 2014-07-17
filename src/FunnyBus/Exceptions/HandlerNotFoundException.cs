using System;

namespace FunnyBus.Exceptions
{
    public class HandlerNotFoundException : Exception
    {
        public HandlerNotFoundException(Type handlerType)
            : base(string.Format("Handler not found for requested message type {0}", handlerType.FullName))
        {
        }
    }
}