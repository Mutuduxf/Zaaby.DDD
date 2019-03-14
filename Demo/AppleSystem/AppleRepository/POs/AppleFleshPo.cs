using System;
using AppleDomain.AggregateRoots;

namespace AppleRepository.POs
{
    public class AppleFleshPo
    {
        public Guid Id { get; set; }
        public int WeightByGram { get; set; }

        public AppleFleshPo(Apple.AppleFlesh flesh)
        {
            Id = flesh.Id;
            WeightByGram = flesh.WeightByGram;
        }
    }
}