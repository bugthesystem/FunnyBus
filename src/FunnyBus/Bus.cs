using System;
using FunnyBus.Exceptions;
using FunnyBus.Infrastructure.Configuration;
using FunnyBus.Infrastructure.IoC;
using FunnyBus.Infrastructure.Reflection;
using FunnyBus.Infrastructure.Store;

namespace FunnyBus
{
    public sealed class Bus : IBus, IConfigutaionContext
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
            AutoScanHandlers = true;

            EnsureSystemInit();
        }

        public static IBus Instance
        {
            get { return LazyInstance.Value; }
        }

        public static void Configure(Action<IConfigutaionContext> context)
        {
            context(Instance as IConfigutaionContext);

            var self = Instance as Bus;
            if (self != null && self.AutoScanHandlers) self.InitRegistry();
        }

        public void Subscribe<THandler>(THandler handler) where THandler : class
        {
            Guard.AgainstNullArgument("handler to subscribe", handler);
            AddToRegistry(handler.GetType());
        }

        public void Subscribe<THandler>() where THandler : class
        {
            AddToRegistry(typeof(THandler));
        }

        public void UnSubscribe<THandler>() where THandler : class
        {
            UnSubscribeImpl(typeof(THandler));
        }

        public void UnSubscribe<THandler>(THandler handler) where THandler : class
        {
            Guard.AgainstNullArgument("handler", handler);

            UnSubscribeImpl(handler.GetType());
        }

        public TResult Publish<TMessage, TResult>(TMessage message) where TMessage : class
        {
            return Publish<TResult>(message);
        }

        public void Publish<TMessage>(TMessage message) where TMessage : class
        {
            Publish((object)message);
        }

        public TResult Publish<TResult>(object message)
        {
            Guard.AgainstNullArgument("message to publish", message);

            Type messageType = message.GetType();
            Type handlerType = _store.GetAsIHandleByMessageType(messageType);

            if (handlerType == null) { throw new NotRegisteredException(messageType); }

            dynamic handlerInstance = IoC.GetService(handlerType);
            return handlerInstance.Handle((dynamic)message);
        }

        public void Publish(object message)
        {
            Guard.AgainstNullArgument("message to publish", message);

            Type messageType = message.GetType();
            Type handlerTypeAsIHandle = _store.GetAsIHandleByMessageType(messageType);

            if (handlerTypeAsIHandle == null) { throw new NotRegisteredException(messageType); }

            dynamic handlerInstance = IoC.GetService(handlerTypeAsIHandle);
            handlerInstance.Handle((dynamic)message);
        }

        private void UnSubscribeImpl(Type key)
        {
            Guard.AgainstNullArgument("key", key);
            _store.Remove(key);
        }

        private void EnsureSystemInit()
        {
            if (!_initCompleted) { InitRegistry(); }
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

        private IFunnyDependencyResolver IoC { get; set; }

        public void SetResolver(IFunnyDependencyResolver funnyDependencyResolver)
        {
            Guard.AgainstNullArgument("funnyDependencyResolver", funnyDependencyResolver);
            IoC = funnyDependencyResolver;
        }

        public bool AutoScanHandlers { private get; set; }
    }
}