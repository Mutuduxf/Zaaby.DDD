using IOrderApplication;
using IOrderApplication.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DemoWeb.Controllers
{
    public class OrderParentApplicationTestController : Controller
    {
        private readonly IOrderParentApplication _freightApplication;

        public OrderParentApplicationTestController(IOrderParentApplication freightApplication)
        {
            _freightApplication = freightApplication;
        }


        public OrderParentDto GetOrderParentDto(string id)
        {
            return _freightApplication.GetOrderParentDto("");
        }

        public string OrderSystemTest()
        {
            return _freightApplication.OrderSystemTest();
        }

        public OrderParentDto Test()
        {
            return _freightApplication.Test();
        }
    }
}