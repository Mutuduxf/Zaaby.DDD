using System.Collections.Generic;

namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IValueObject<in T> : IEqualityComparer<T>
    {
        bool Equals(object obj);
        int GetHashCode();
    }
}