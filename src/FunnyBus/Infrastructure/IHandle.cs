namespace FunnyBus.Infrastructure
{
    public interface IHandle<in TMessage, out TResult>
    {
        TResult Handle(TMessage message);
    }

    public interface IHandle<in TMessage>
    {
        void Handle(TMessage message);
    }
}