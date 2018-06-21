namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    /// <summary>
    /// This message type will republish to dead letter queue when throw a exception.
    /// </summary>
    public interface IEvent
    {
        string Version { get; set; }
    }
}