using System;

namespace FunnyBus
{
    public interface IBus
    {
        void Subscribe<THandler>(THandler handler) where THandler : class;

        void Subscribe<THandler>() where THandler : class;

        void Subscribe<TMessage>(Action<TMessage> handler) where TMessage : class;

        void UnSubscribe<T>() where T : class;

        void UnSubscribe<THandler>(THandler handler) where THandler : class;

        TResult Publish<TMessage, TResult>(TMessage message) where TMessage : class;

        void Publish<TMessage>(TMessage message) where TMessage : class;

        TResult Publish<TResult>(object message);

        void Publish(object message);
    }
}