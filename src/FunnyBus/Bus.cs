using System;
using FunnyBus.Exceptions;
using FunnyBus.Infrastructure.Configuration;
using FunnyBus.Infrastructure.IoC;
using FunnyBus.Infrastructure.Reflection;
using FunnyBus.Infrastructure.Store;

namespace FunnyBus
{
    public sealed class Bus : IBus, IConfigurationContext
    {
        private readonly IHandlerStore _store;
        private readonly IHandlerScanner _handlerScanner;

        private bool _initCompleted;
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
            AutoScanHandlers = false;

            EnsureSystemInit();
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
            context(Instance as IConfigurationContext);

            var self = Instance as Bus;
            if (self != null && self.AutoScanHandlers) self.InitRegistry();
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
        /// <typeparam name="THandler"></typeparam>
        public void UnSubscribe<THandler>() where THandler : class
        {
            UnSubscribeImpl(typeof(THandler));
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

            if (handlerType == null) { throw new NotRegisteredException(messageType); }

            dynamic handlerInstance = IoC.GetService(handlerType);
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
            Type handlerTypeAsIHandle = _store.GetAsIHandleByMessageType(messageType);

            if (handlerTypeAsIHandle == null) { throw new NotRegisteredException(messageType); }

            dynamic handlerInstance = IoC.GetService(handlerTypeAsIHandle);
            handlerInstance.Handle((dynamic)message);
        }

        internal IFunnyDependencyResolver IoC { get; set; }

        #region IConfigutaionContext implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funnyDependencyResolver"></param>
        public void SetResolver(IFunnyDependencyResolver funnyDependencyResolver)
        {
            Guard.AgainstNullArgument("funnyDependencyResolver", funnyDependencyResolver);
            IoC = funnyDependencyResolver;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AutoScanHandlers { private get; set; }

        #endregion

        private void UnSubscribeImpl(Type key)
        {
            Guard.AgainstNullArgument("key", key);
            _store.Remove(key);
        }

        private void EnsureSystemInit()
        {
            if (!_initCompleted && AutoScanHandlers) { InitRegistry(); }
        }

        private void InitRegistry()
        {
            _handlerScanner.RegisterHandlerDefinitions(AddToRegistry);
            _initCompleted = true;
        }

        private void AddToRegistry(Type handler)
        {
            Guard.AgainstNullArgument("handler", handler);
            _store.Add(handler);
        }
    }
}