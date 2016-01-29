using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace FunnyBus.Infrastructure.Store
{
    internal class HandlerStore : IHandlerStore
    {
        private readonly ConcurrentDictionary<Type, List<IHandleDefinition>> _typeRegistry = new ConcurrentDictionary<Type, List<IHandleDefinition>>();

        public Type Get(Type handlerType)
        {
            return _typeRegistry.SingleOrDefault(pair => pair.Key == handlerType).Key;
        }

        public bool Remove(Type key)
        {
            bool result = false;

            if (Get(key) != null)
            {
                List<IHandleDefinition> item;

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
            foreach (KeyValuePair<Type, List<IHandleDefinition>> pair in _typeRegistry)
            {
                IHandleDefinition handleDefinition = pair.Value.SingleOrDefault(def => def.MessageType == messageType);
                if (handleDefinition != null)
                {
                    TypedHandlerDefinition definition = (TypedHandlerDefinition)handleDefinition;

                    if (definition.HasReturnType && definition.ReturnType != null)
                    {
                        result = typeof(IHandle<,>).MakeGenericType(messageType, definition.ReturnType);
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

        public bool AddActionHandler(Type messageType, Action<object> proxyaAction)
        {
            var handlerDefinition = new ActionHandlerDefinition { ProxyAction = proxyaAction, MessageType = messageType };

            if (_typeRegistry.ContainsKey(messageType))
            {
                _typeRegistry[messageType].Add(handlerDefinition);
            }
            else
            {
                return _typeRegistry.TryAdd(messageType, new List<IHandleDefinition> { handlerDefinition });
            }

            return true;
        }

        public bool IsActionHandler(Type messageType)
        {
            return _typeRegistry.ContainsKey(messageType);
        }

        public List<ActionHandlerDefinition> GetActionHandlerDefinitionsByMessageType(Type messageType)
        {
            return _typeRegistry[messageType].ConvertAll(input => (ActionHandlerDefinition)input);
        }

        private List<IHandleDefinition> GetHandleDefinitions(Type typeToWatch)
        {
            return typeToWatch.GetInterfaces()
                .Where(type => type.IsGenericType)
                .Select(type =>
                {
                    Type[] genericArguments = type.GetGenericArguments();

                    return new TypedHandlerDefinition
                                    {
                                        MessageType = genericArguments.First(),
                                        MessageFullName = genericArguments.First().FullName,
                                        ReturnType = genericArguments.Length > 1 ? genericArguments.Last() : null,
                                        HasReturnType = genericArguments.Length > 1
                                    };
                }).Cast<IHandleDefinition>().ToList();
        }
    }
}