using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/disposition-types")]
    
    public class DispositionTypesController : ControllerBase
    {
        private readonly IDispositionTypeService _service;

        public DispositionTypesController(
            IDispositionTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response =
                await _service.GetAllAsync();

            return this.ToHttpResponse(response);
        }
    }
}