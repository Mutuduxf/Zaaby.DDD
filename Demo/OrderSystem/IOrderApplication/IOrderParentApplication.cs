using IOrderApplication.DTOs;
using Zaaby.DDD.Abstractions.Application;

namespace IOrderApplication
{
    public interface IOrderParentApplication : IApplicationService
    {
        OrderParentDto GetOrderParentDto(string id);
        string OrderSystemTest();
        OrderParentDto Test();
        int PublishEvent(int quantity);
    }
}