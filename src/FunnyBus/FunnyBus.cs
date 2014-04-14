using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FunnyBus.Exceptions;
using FunnyBus.Infrastructure.Configuration;
using FunnyBus.Infrastructure.IoC;
using FunnyBus.Infrastructure.Reflection;
using FunnyBus.Infrastructure.Store;

namespace FunnyBus
{
    public sealed class FunnyBus : IFunnyBus, IConfigutaionContext
    {
        private readonly IHandlerStore _store;
        private readonly IHandlerScanner _handlerScanner;
        private readonly IHandleMethodFinder _handleMethodFinder;

        private bool _initCompleted;
        private static readonly Lazy<IFunnyBus> LazyInstance = new Lazy<IFunnyBus>(() => new FunnyBus(), true);

        public FunnyBus()
            : this(new HandlerStore())
        {

        }

        internal FunnyBus(IHandlerStore store)
            : this(store, new HandlerScanner(), new HandleMethodFinder())
        {

        }

        internal FunnyBus(IHandlerStore store, HandlerScanner handlerScanner, HandleMethodFinder handleMethodFinder)
        {
            _store = store;
            _handlerScanner = handlerScanner;
            _handleMethodFinder = handleMethodFinder;

            EnsureSystemInit();
        }

        public static IFunnyBus Instance
        {
            get { return LazyInstance.Value; }
        }

        public static void Configure(Action<IConfigutaionContext> context)
        {
            context(Instance as IConfigutaionContext);

            var self = Instance as FunnyBus;
            if (self != null) self.InitRegistry();
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
            Type handlerType = _store.GetByMessageType(messageType);

            if (handlerType == null) { throw new NotRegisteredException(messageType); }

            object handlerInstance = IoC.GetService(handlerType); //TODO IHandle<T,K>
            MethodInfo handleMethod = _handleMethodFinder.Find(handlerType, messageType);

            if (handleMethod != null)
            {
                return (TResult)handleMethod.Invoke(handlerInstance, new[] { message });
            }

            return default(TResult);
        }

        public void Publish(object message)
        {
            Guard.AgainstNullArgument("message to publish", message);

            Type messageType = message.GetType();
            Type handlerType = _store.GetByMessageType(messageType);

            if (handlerType == null) { throw new NotRegisteredException(messageType); }

            object handlerInstance = IoC.GetService(handlerType);

            IEnumerable<MethodInfo> handleMethods = _handleMethodFinder.FindAll(handlerType, messageType);

            handleMethods.ToList().ForEach(handleMethod => handleMethod.Invoke(handlerInstance, new[] { message }));
        }

        private void UnSubscribeImpl(Type key)
        {
            Guard.AgainstNullArgument("key", key);
            _store.Remove(key);
        }

        private void EnsureSystemInit()
        {
            if (IoC == null)
            {
                //TODO: Not working for now..
                IPoorDependencyContainer container = new PoorDependencyContainer();
                container.Bind<IFunnyBus>(() => Instance);
                IoC = new DefaultDependencyResolverAdapter(container);
            }
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

        private IDependencyResolverAdapter IoC { get; set; }

        public void SetResolverAdapter(IDependencyResolverAdapter dependencyResolverAdapter)
        {
            Guard.AgainstNullArgument("dependencyResolverAdapter", dependencyResolverAdapter);
            IoC = dependencyResolverAdapter;
        }
    }
}