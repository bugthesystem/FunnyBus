using System;
using System.Collections.Generic;

namespace FunnyBus.Infrastructure.Store
{
    internal interface IHandlerStore
    {
        Type Get(Type handlerType);
        Type GetAsIHandleByMessageType(Type messageType);
        Type GetByMessageType(Type messageType);

        bool Remove(Type key);

        bool Add(Type handler);

        bool AddActionHandler(Type messageType, Action<object> proxyaAction);
        bool IsActionHandler(Type messageType);
        List<ActionHandlerDefinition> GetActionHandlerDefinitionsByMessageType(Type messageType);
    }
}