using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunnyBus.Exceptions;
using FunnyBus.Infrastructure;
using FunnyBus.Infrastructure.Store;
using FunnyBus.Infrastructure.Reflection;
using FunnyBus.Infrastructure.Configuration;
using FunnyBus.Infrastructure.DependencyInjection;

namespace FunnyBus
{
    public sealed class Bus : IBus, IConfigurationContext
    {
        private readonly IHandlerStore _store;
        private readonly IHandlerScanner _handlerScanner;

        private static readonly Lazy<IBus> LazyInstance = new Lazy<IBus>(() => new Bus(), true);

        public Bus()
            : this(new HandlerStore())
        {

        }

        internal Bus(IHandlerStore store)
            : this(store, new HandlerScanner())
        {

        }

        internal Bus(IHandlerStore store, HandlerScanner handlerScanner)
        {
            _store = store;
            _handlerScanner = handlerScanner;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IBus Instance
        {
            get { return LazyInstance.Value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public static void Configure(Action<IConfigurationContext> context)
        {
            var configurationContext = Instance as IConfigurationContext;

            if (configurationContext != null)
            {
                configurationContext.AutoScanHandlers = true;
                context(configurationContext);

                var self = Instance as Bus;
                if (self != null && self.AutoScanHandlers)
                {
                    self.InitRegistry();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="handler"></param>
        public void Subscribe<THandler>(THandler handler) where THandler : class
        {
            Guard.AgainstNullArgument("handler to subscribe", handler);
            AddToRegistry(handler.GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        public void Subscribe<THandler>() where THandler : class
        {
            AddToRegistry(typeof(THandler));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="handler"></param>
        public void Subscribe<TMessage>(Action<TMessage> handler) where TMessage : class
        {
            _store.AddActionHandler(typeof(TMessage), o => handler((TMessage)o));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnSubscribe<T>() where T : class
        {
            UnSubscribeImpl(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="handler"></param>
        public void UnSubscribe<THandler>(THandler handler) where THandler : class
        {
            Guard.AgainstNullArgument("handler", handler);

            UnSubscribeImpl(handler.GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public TResult Publish<TMessage, TResult>(TMessage message) where TMessage : class
        {
            return Publish<TResult>(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message"></param>
        public void Publish<TMessage>(TMessage message) where TMessage : class
        {
            Publish((object)message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public TResult Publish<TResult>(object message)
        {
            Guard.AgainstNullArgument("message to publish", message);

            Type messageType = message.GetType();
            Type handlerType = _store.GetAsIHandleByMessageType(messageType);

            if (handlerType == null) { throw new HandlerNotFoundException(messageType); }

            dynamic handlerInstance = DependencyResolver.GetService(handlerType);
            return handlerInstance.Handle((dynamic)message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Publish(object message)
        {
            Guard.AgainstNullArgument("message to publish", message);

            Type messageType = message.GetType();

            if (_store.IsActionHandler(messageType))
            {
                List<ActionHandlerDefinition> handlerDefinitions = _store.GetActionHandlerDefinitionsByMessageType(messageType);

                if (handlerDefinitions != null && handlerDefinitions.Any())
                {
                    if (ParallelHandlerExecution)
                    {
                        Parallel.ForEach(handlerDefinitions, (handlerDefinition) => handlerDefinition.ProxyAction(message));
                    }
                    else
                    {
                        foreach (ActionHandlerDefinition actionHandlerDefinition in handlerDefinitions)
                        {
                            actionHandlerDefinition.ProxyAction(message);
                        }
                    }
                }
            }
            else
            {
                Type handlerTypeAsIHandle = _store.GetAsIHandleByMessageType(messageType);

                if (handlerTypeAsIHandle == null) { throw new HandlerNotFoundException(messageType); }

                IEnumerable<dynamic> handlers = DependencyResolver.GetServices(handlerTypeAsIHandle);

                if (handlers != null && handlers.Any())
                {
                    if (ParallelHandlerExecution)
                    {
                        Parallel.ForEach(handlers, (handler) => handler.Handle((dynamic)message));
                    }
                    else
                    {
                        foreach (dynamic handler in handlers)
                        {
                            handler.Handle((dynamic)message);
                        }
                    }
                }
            }
        }

        internal IFunnyDependencyResolver DependencyResolver { get; set; }

        #region IConfigutaionContext implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funnyDependencyResolver"></param>
        public void SetResolver(IFunnyDependencyResolver funnyDependencyResolver)
        {
            Guard.AgainstNullArgument("funnyDependencyResolver", funnyDependencyResolver);
            DependencyResolver = funnyDependencyResolver;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AutoScanHandlers { private get; set; }

        public bool ParallelHandlerExecution { private get; set; }

        #endregion

        private void UnSubscribeImpl(Type key)
        {
            Guard.AgainstNullArgument("key", key);
            _store.Remove(key);
        }

        private void InitRegistry()
        {
            _handlerScanner.RegisterHandlerDefinitions(AddToRegistry);
        }

        private void AddToRegistry(Type handler)
        {
            Guard.AgainstNullArgument("handler", handler);
            _store.Add(handler);
        }
    }
}