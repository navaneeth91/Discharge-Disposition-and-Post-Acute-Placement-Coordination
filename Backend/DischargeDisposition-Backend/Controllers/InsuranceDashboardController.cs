
using DischargeDisposition_Backend.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DischargeDisposition_Backend.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize(
    Roles =
    "Authorization Coordinator")]
    public class InsuranceDashboardController
    : ControllerBase
    {
        private readonly
            IInsuranceDashboardService
            _service;

        public InsuranceDashboardController(
            IInsuranceDashboardService service)
        {
            _service = service;
        }
    }
}