using System;

namespace FunnyBus.Infrastructure.Store
{
    internal interface IHandlerStore
    {
        Type Check(Type type);
        Type CheckByMessageType(Type messageType);

        bool Remove(Type key);

        bool Add(Type handler);
    }
}