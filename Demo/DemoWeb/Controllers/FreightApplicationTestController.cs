using IShippingApplication;
using IShippingApplication.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DemoWeb.Controllers
{
    public class FreightApplicationTestController : Controller
    {
        private readonly IFreightApplication _freightApplication;

        public FreightApplicationTestController(IFreightApplication freightApplication)
        {
            _freightApplication = freightApplication;
        }
        
        public int FreightCharge()
        {
            return _freightApplication.FreightCharge(new Cargo());
        }

        public string ShippiingSystemTest()
        {
            return _freightApplication.ShippiingSystemTest();
        }
    }
}