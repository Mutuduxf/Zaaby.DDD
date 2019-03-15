using System;
using System.Collections.Generic;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.AggregateRoots
{
    public class Apple : IAggregateRootWithGuidKey
    {
        public Guid Id { get; protected set; }

        public int WeightByGram => Flesh.WeightByGram + 1;

        public bool CanBeEat => Skin.Color == AppleSkin.AppleColor.Red;

        private AppleSkin _skin;

        public AppleSkin Skin
        {
            get => _skin ?? (_skin = new AppleSkin());
            protected set => _skin = value;
        }

        private AppleFlesh _flesh;

        public AppleFlesh Flesh
        {
            get => _flesh ?? (_flesh = new AppleFlesh());
            protected set => _flesh = value;
        }

        private List<AppleCore> _cores;

        public List<AppleCore> Cores
        {
            get => _cores ?? (_cores = new List<AppleCore>());
            protected set => _cores = value;
        }

        public Apple(Guid id)
        {
            Id = id;
        }

        public int Water()
        {
            return ++Flesh.WeightByGram;
        }

        public AppleSkin.AppleColor Grow()
        {
            switch (Skin.Color)
            {
                case AppleSkin.AppleColor.Green:
                    Skin.Color = AppleSkin.AppleColor.Yellow;
                    return Skin.Color;
                case AppleSkin.AppleColor.Yellow:
                    Skin.Color = AppleSkin.AppleColor.Red;
                    return Skin.Color;
                case AppleSkin.AppleColor.Red:
                    return Skin.Color;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class AppleSkin
    {
        public Guid Id { get; set; }
        public AppleColor Color { get; set; }

        public enum AppleColor
        {
            Green,
            Yellow,
            Red
        }
    }

    public class AppleFlesh
    {
        public Guid Id { get; set; }
        public int WeightByGram { get; set; }
    }

    public class AppleCore
    {
        public Guid Id { get; set; }
    }
}