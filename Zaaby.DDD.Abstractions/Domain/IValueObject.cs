using System.Collections.Generic;

namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IValueObject : IEqualityComparer<IValueObject>
    {

    }
}