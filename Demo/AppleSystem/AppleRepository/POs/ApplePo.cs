using System;
using AppleDomain.AggregateRoots;

namespace AppleRepository.POs
{
    public class ApplePo
    {
        public Guid Id { get; set; }

        public ApplePo(Apple apple)
        {
            Id = apple.Id;
        }
    }
}