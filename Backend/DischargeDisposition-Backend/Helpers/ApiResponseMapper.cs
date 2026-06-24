using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
namespace DischargeDisposition_Backend.Helpers
{
    public static class ApiResponseMapper
    {
        public static IActionResult ToHttpResponse<T>(
            this ControllerBase controller,
            ApiResponse<T> response)
        {
            return response.StatusCode switch
            {
                200 => controller.Ok(response),
                201 => controller.StatusCode(201, response),
                204 => controller.NoContent(),
                400 => controller.BadRequest(response),
                401 => controller.Unauthorized(response),
                404 => controller.NotFound(response),
                500 => controller.StatusCode(500, response),
                _ => controller.StatusCode(response.StatusCode, response)
            };
        }
    }
}
