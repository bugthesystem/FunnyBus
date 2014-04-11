using System;
using System.Collections.Generic;
using System.Reflection;

namespace FunnyBus.Infrastructure.Reflection
{
    public interface IHandleMethodFinder
    {
        MethodInfo Find(Type handlerType, Type messageType);
        IEnumerable<MethodInfo> FindAll(Type handlerType, Type messageType);
    }
}