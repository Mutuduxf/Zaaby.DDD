using System;
using AppleDomain.Aggregates.Entities;

namespace AppleRepository.POs
{
    public class AppleSkinPo
    {
        public Guid Id { get; set; }
        public Guid AppleId { get; set; }
        public AppleSkinState.AppleColor Color { get; set; }
    }
}