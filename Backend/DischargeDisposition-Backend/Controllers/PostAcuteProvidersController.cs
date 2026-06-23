using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Hospital.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/post-acute-providers")]
    public class PostAcuteProvidersController
        : ControllerBase
    {
        private readonly
            IPostAcuteProviderService
            _service;

        public PostAcuteProvidersController(
            IPostAcuteProviderService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var providers =
                await _service.GetAllAsync();

            return Ok(providers);
        }

        [HttpGet("{providerId:int}")]
        public async Task<IActionResult>
            GetProvider(int providerId)
        {
            var provider =
                await _service
                    .GetByIdAsync(providerId);

            if (provider == null)
                return NotFound();

            return Ok(provider);
        }

        [HttpGet("disposition/{dispositionTypeId:int}")]
        public async Task<IActionResult>
            GetByDisposition(
                int dispositionTypeId)
        {
            var providers =
                await _service
                    .GetByDispositionTypeAsync(
                        dispositionTypeId);

            return Ok(providers);
        }
    }
}