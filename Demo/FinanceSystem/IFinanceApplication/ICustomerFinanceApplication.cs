using IFinanceApplication.DTOs;
using System;
using Zaaby.DDD.Abstractions.Application;

namespace IFinanceApplication
{
    public interface ICustomerFinanceApplication : IApplicationService
    {
        bool Charge(CustomerChargeParam param);

        string FinanceSystemTest();

        CustomerDto GetCustomer(Guid id);
    }
}