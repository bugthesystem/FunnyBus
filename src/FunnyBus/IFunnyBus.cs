namespace FunnyBus
{
    public interface IFunnyBus
    {
        void Subscribe<THandler>(THandler handler) where THandler : class;
        void Subscribe<THandler>() where THandler : class;

        void UnSubscribe<THandler>() where THandler : class;
        void UnSubscribe<THandler>(THandler handler) where THandler : class;

        TResult Publish<TMessage, TResult>(TMessage message) where TMessage : class;
        void Publish<TMessage>(TMessage message) where TMessage : class;

        TResult Publish<TResult>(object message);
        void Publish(object message);
    }
}