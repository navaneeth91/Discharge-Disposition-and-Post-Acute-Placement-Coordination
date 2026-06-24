using DischargeDisposition_Backend.Helpers;
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
        public async Task<IActionResult>
            GetAll()
        {
            var response =
                await _service.GetAllAsync();

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("{providerId:int}")]
        public async Task<IActionResult>
            GetProvider(
                int providerId)
        {
            var response =
                await _service
                    .GetByIdAsync(
                        providerId);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("disposition/{dispositionTypeId:int}")]
        public async Task<IActionResult>
            GetByDisposition(
                int dispositionTypeId)
        {
            var response =
                await _service
                    .GetByDispositionTypeAsync(
                        dispositionTypeId);

            return this
                .ToHttpResponse(response);
        }
    }
}