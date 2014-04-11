using System;

namespace FunnyBus.Infrastructure.Reflection
{
    internal interface IHandlerScanner
    {
        bool RegisterHandlerDefinitions(Action<Type> addToRegistry);
    }
}