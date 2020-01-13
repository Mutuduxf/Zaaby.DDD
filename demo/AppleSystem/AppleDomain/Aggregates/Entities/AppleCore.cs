using System;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.Aggregates.Entities
{
    public class AppleCore : IValueObject
    {
        private readonly AppleCoreState _state;

        public AppleCore(AppleCoreState state)
        {
            _state = state ?? new AppleCoreState();
        }

        public Guid Id
        {
            get => _state.Id;
            protected set => _state.Id = value;
        }
    }

    public class AppleCoreState
    {
        public Guid Id { get; set; }
    }
}