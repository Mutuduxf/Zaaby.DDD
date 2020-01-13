using System;

namespace AppleDomain.Aggregates.Entities
{

    public class AppleFlesh
    {
        private readonly AppleFleshState _state;

        public AppleFlesh(AppleFleshState state)
        {
            _state = state ?? new AppleFleshState();
        }

        public Guid Id
        {
            get => _state.Id;
            protected set => _state.Id = value;
        }

        public int WeightByGram
        {
            get => _state.WeightByGram;
            protected set => _state.WeightByGram = value;
        }

        public int WeightIncrease(int weight)
        {
            return WeightByGram = WeightByGram + weight;
        }
    }

    public class AppleFleshState
    {
        public Guid Id { get; set; }
        public int WeightByGram { get; set; }
    }
}