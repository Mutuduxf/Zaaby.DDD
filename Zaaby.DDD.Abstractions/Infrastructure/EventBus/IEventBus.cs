using System;

namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    public interface IEventBus : IDisposable
    {
        void PublishEvent<T>(T @event) where T : IEvent;
        void PublishEvent(string eventName, byte[] body);

        /// <summary>
        /// The subscriber cluster will receive the event by the default queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void ReceiveEvent<T>(Action<T> handle) where T : IEvent;

        /// <summary>
        /// The subscriber cluster will receive the event by its own queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void SubscribeEvent<T>(Action<T> handle) where T : IEvent;
    }
}