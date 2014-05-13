using System;

namespace FunnyBus.Exceptions
{
    public class ResolutionException : Exception
    {
        public ResolutionException(Type type)
            : base(string.Format("Resolve failed for requested type {0}", type.FullName))
        {
        }
    }
    public class NotRegisteredException : Exception
    {
        public NotRegisteredException(Type type)
            : base(string.Format("Handler not found for requested message type {0}", type.FullName))
        {
        }
    }
}