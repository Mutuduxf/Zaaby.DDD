using System;
using IFinanceApplication;
using IFinanceApplication.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DemoWeb.Controllers
{
    public class CustomerFinanceApplicationTestController : Controller
    {
        private readonly ICustomerFinanceApplication _customerFinanceApplication;

        public CustomerFinanceApplicationTestController(ICustomerFinanceApplication customerFinanceApplication)
        {
            _customerFinanceApplication = customerFinanceApplication;
        }

        public bool Charge()
        {
            return _customerFinanceApplication.Charge(new CustomerChargeParam());
        }

        public string FinanceSystemTest()
        {
            return _customerFinanceApplication.FinanceSystemTest();
        }

        public CustomerDto GetCustomer()
        {
            return _customerFinanceApplication.GetCustomer(Guid.NewGuid());
        }
    }
}