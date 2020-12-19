using System;
using System.Collections.Generic;
using AppleDomain.Aggregates.Entities;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.Aggregates
{
    public class Apple : AggregateRootWithGuidKey
    {
        private readonly AppleState _state;

        public Apple(AppleState state)
        {
            _state = state ?? new AppleState();
        }

        public Guid Id
        {
            get => _state.Id;
            protected set => _state.Id = value;
        }

        public int WeightByGram => Flesh.WeightByGram + 1;

        public bool CanBeEat => Skin.Color == AppleSkinState.AppleColor.Red;

        public AppleSkin Skin
        {
            get => _state.Skin;
            protected set => _state.Skin = value;
        }

        public AppleFlesh Flesh
        {
            get => _state.Flesh;
            protected set => _state.Flesh = value;
        }

        public List<AppleCore> Cores
        {
            get => _state.Cores;
            protected set => _state.Cores = value;
        }

        public Apple(Guid id)
        {
            Id = id;
        }

        public int Water()
        {
            return Flesh.WeightIncrease(1);
        }

        public AppleSkinState.AppleColor Grow()
        {
            switch (Skin.Color)
            {
                case AppleSkinState.AppleColor.Green:
                    Skin.SetColor(AppleSkinState.AppleColor.Yellow);
                    return Skin.Color;
                case AppleSkinState.AppleColor.Yellow:
                    Skin.SetColor(AppleSkinState.AppleColor.Red);
                    return Skin.Color;
                case AppleSkinState.AppleColor.Red:
                    return Skin.Color;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class AppleState
    {
        public Guid Id { get; set; }
        public AppleSkin Skin { get; set; }
        public AppleFlesh Flesh { get; set; }
        public List<AppleCore> Cores { get; set; }
    }
}