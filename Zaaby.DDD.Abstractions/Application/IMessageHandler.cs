namespace Zaaby.DDD.Abstractions.Application
{
    public interface IMessageHandler
    {
    }

    public interface IMessageHandler<in TMessage> : IMessageHandler
    {
        void Handle(TMessage message);
    }
}