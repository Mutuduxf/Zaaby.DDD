using System;
using AppleDomain.AggregateRoots;

namespace AppleRepository.POs
{
    public class AppleSkinPo
    {
        public Guid Id { get; set; }
        public Apple.AppleSkin.AppleColor Color { get; set; }

        public AppleSkinPo(Apple.AppleSkin skin)
        {
            Id = skin.Id;
            Color = skin.Color;
        }
    }
}