using System;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.Aggregates.Entities
{
    public class AppleCore:IValueObject<AppleCore>
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

        public bool Equals(AppleCore x, AppleCore y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(AppleCore obj)
        {
            throw new NotImplementedException();
        }
    }

    public class AppleCoreState
    {
        public Guid Id { get; set; }
    }
}