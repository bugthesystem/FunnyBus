using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FunnyBus.Infrastructure.Models;

namespace FunnyBus.Infrastructure.Store
{
    internal class HandlerStore : IHandlerStore
    {
        private readonly ConcurrentDictionary<Type, List<HandleDefinition>> _typeRegistry = new ConcurrentDictionary<Type, List<HandleDefinition>>();

        public Type Check(Type type)
        {
            return _typeRegistry.SingleOrDefault(pair => pair.Key == type).Key;
        }

        public bool Remove(Type key)
        {
            bool result = false;
            
            if (Check(key) != null)
            {
                List<HandleDefinition> item;

                if(_typeRegistry.TryRemove(key, out item))
                {
                    result= true;
                }
            }

            return result;
        }

        public Type CheckByMessageType(Type messageType)
        {
            return _typeRegistry.SingleOrDefault(pair => pair.Value.Find(def => def.MessageType == messageType) != null).Key;
        }

        public bool Add(Type handler)
        {
            if (Check(handler) == null)
            {
                return _typeRegistry.TryAdd(handler, GetHandleDefinitions(handler));
            }

            return false;
        }

        private List<HandleDefinition> GetHandleDefinitions(Type typeToWatch)
        {
            return typeToWatch.GetInterfaces()
                .Where(type => type.IsGenericType)
                .Select(type => new HandleDefinition
                {
                    MessageType = type.GenericTypeArguments.First(),
                    MessageFullName = type.GenericTypeArguments.First().FullName
                }).ToList();
        }
    }
}