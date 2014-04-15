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

        public Type Get(Type type)
        {
            return _typeRegistry.SingleOrDefault(pair => pair.Key == type).Key;
        }

        public bool Remove(Type key)
        {
            bool result = false;

            if (Get(key) != null)
            {
                List<HandleDefinition> item;

                if (_typeRegistry.TryRemove(key, out item))
                {
                    result = true;
                }
            }

            return result;
        }

        public Type GetAsIHandleByMessageType(Type messageType)
        {
            Type result = null;
            foreach (KeyValuePair<Type, List<HandleDefinition>> pair in _typeRegistry)
            {
                HandleDefinition handleDefinition = pair.Value.SingleOrDefault(def => def.MessageType == messageType);
                if (handleDefinition != null)
                {
                    if (handleDefinition.HasReturnType && handleDefinition.ReturnType != null)
                    {
                        result = typeof(IHandle<,>).MakeGenericType(messageType, handleDefinition.ReturnType);
                    }
                    else
                    {
                        result = typeof(IHandle<>).MakeGenericType(messageType);
                    }
                    break;
                }
            }
            return result;
        }

        public Type GetByMessageType(Type messageType)
        {
            return _typeRegistry.SingleOrDefault(pair => pair.Value.Find(def => def.MessageType == messageType) != null).Key;
        }

        public bool Add(Type handler)
        {
            if (Get(handler) == null)
            {
                return _typeRegistry.TryAdd(handler, GetHandleDefinitions(handler));
            }

            return false;
        }

        private List<HandleDefinition> GetHandleDefinitions(Type typeToWatch)
        {
            //#if NET40
            return typeToWatch.GetInterfaces()
                .Where(type => type.IsGenericType)
                .Select(type =>
                {
                    Type[] genericArguments = type.GetGenericArguments();

                    return new HandleDefinition
                                    {
                                        MessageType = genericArguments.First(),
                                        MessageFullName = genericArguments.First().FullName,
                                        ReturnType = genericArguments.Length > 1 ? genericArguments.Last() : null,
                                        HasReturnType = genericArguments.Length > 1
                                    };
                }).ToList();
            //#else
            //            return typeToWatch.GetInterfaces()
            //                .Where(type => type.IsGenericType)
            //                .Select(type => new HandleDefinition
            //                {
            //                    MessageType = type.GenericTypeArguments.First(),
            //                    MessageFullName = type.GenericTypeArguments.First().FullName
            //                }).ToList();
            //#endif
        }
    }
}