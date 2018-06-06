namespace Zaaby.DDD.Abstractions.Application
{
    public interface IMessageHandler<in TMessage>
    {
        void Handle(TMessage message);
    }
}