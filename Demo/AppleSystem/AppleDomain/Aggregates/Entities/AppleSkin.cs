using System;

namespace AppleDomain.Aggregates.Entities
{

    public class AppleSkin
    {
        private readonly AppleSkinState _state;

        public AppleSkin(AppleSkinState state)
        {
            _state = state ?? new AppleSkinState();
        }

        public Guid Id
        {
            get => _state.Id;
            protected set => _state.Id = value;
        }

        public AppleSkinState.AppleColor Color
        {
            get => _state.Color;
            protected set => _state.Color = Color;
        }

        public AppleSkinState.AppleColor SetColor(AppleSkinState.AppleColor color)
        {
            return Color = color;
        }
    }

    public class AppleSkinState
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
}