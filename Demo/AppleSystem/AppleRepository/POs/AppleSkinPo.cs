using System;
using AppleDomain.AggregateRoots;

namespace AppleRepository.POs
{
    public class AppleSkinPo
    {
        public Guid Id { get; set; }
        public Guid AppleId { get; set; }
        public AppleSkin.AppleColor Color { get; set; }
    }
}