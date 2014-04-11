using System;

namespace FunnyBus.Exceptions
{
    public class ResolutionException : Exception
    {
        public ResolutionException(Type type)
            : base(string.Format("Resolve failed for type {0}", type.FullName))
        {
        }
    }
    public class NotRegisteredException : Exception
    {
        public NotRegisteredException(Type type)
            : base(string.Format("Not found handler for message type {0}", type.FullName))
        {
        }
    }
}