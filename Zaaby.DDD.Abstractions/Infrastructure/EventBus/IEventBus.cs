using System;

namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    public interface IEventBus
    {
        void PublishEvent<T>(T @event) where T : IEvent;

        /// <summary>
        /// The subscriber cluster will receive the event by its own queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void SubscribeEvent<T>(Action<T> handle) where T : IEvent;
    }
}