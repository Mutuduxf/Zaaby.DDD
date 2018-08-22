﻿namespace Zaaby.DDD.Abstractions.Application
{
    public interface IIntegrationEventHandler
    {
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IIntegrationEvent
    {
        void Handle(TIntegrationEvent integrationEvent);
    }
}