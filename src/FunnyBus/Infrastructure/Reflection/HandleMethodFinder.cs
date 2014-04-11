using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FunnyBus.Infrastructure.Reflection
{
    public class HandleMethodFinder : IHandleMethodFinder
    {
        public MethodInfo Find(Type handlerType, Type messageType)
        {
            return handlerType
                .GetMethods()
                .Where((methodInfo, i) => methodInfo.GetParameters().Any() && methodInfo.GetParameters().First().ParameterType == messageType)
                .FirstOrDefault();
        }

        public IEnumerable<MethodInfo> FindAll(Type handlerType, Type messageType)
        {
            return handlerType
                .GetMethods()
                .Where((methodInfo, i) => methodInfo.GetParameters().Any() && methodInfo.GetParameters().First().ParameterType == messageType);
        }
    }
}