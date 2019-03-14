using System;
using AppleDomain.AggregateRoots;

namespace AppleRepository.POs
{
    public class AppleCorePo
    {
        public Guid Id { get; set; }

        public AppleCorePo(Apple.AppleCore core)
        {
            Id = core.Id;
        }
    }
}