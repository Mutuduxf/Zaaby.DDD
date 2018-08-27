using System;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.AggregateRoots
{
    public class Apple : IAggregateRootWithGuidKey
    {
        public Guid Id { get; }
    }
}