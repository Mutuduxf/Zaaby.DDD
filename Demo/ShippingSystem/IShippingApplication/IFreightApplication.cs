using IShippingApplication.DTOs;
using Zaaby.DDD.Abstractions.Application;

namespace IShippingApplication
{
    public interface IFreightApplication : IApplicationService
    {
        int FreightCharge(Cargo cargo);
        string ShippiingSystemTest();
    }
}