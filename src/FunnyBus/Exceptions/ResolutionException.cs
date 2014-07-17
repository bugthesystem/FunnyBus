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
}