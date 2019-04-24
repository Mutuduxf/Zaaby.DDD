using System;

namespace AppleRepository.POs
{
    public class AppleFleshPo
    {
        public Guid Id { get; set; }
        public Guid AppleId { get; set; }
        public int WeightByGram { get; set; }
    }
}