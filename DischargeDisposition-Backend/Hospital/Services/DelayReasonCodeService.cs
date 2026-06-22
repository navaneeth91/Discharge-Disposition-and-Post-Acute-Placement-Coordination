using DischargeDisposition_Backend.Hospital.Models;
﻿using DischargeDisposition_Backend.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DelayReasonCodeService : IDelayReasonCodeService
    public class DelayReasonCodeService
        : IDelayReasonCodeService
    {
        private readonly IDelayReasonCodeRepository _repository;

        public DelayReasonCodeService(
            IDelayReasonCodeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DelayReasonCode>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        public async Task<ApiResponse<List<DelayReasonCodeResponse>>>
            GetAllAsync()
        {
            var reasons =
                await _repository.GetAllAsync();

            var result = reasons
                .Select(x => new DelayReasonCodeResponse
                {
                    Id = x.Id,
                    ReasonName = x.ReasonName
                })
                .ToList();

            return new ApiResponse<List<DelayReasonCodeResponse>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Delay reasons retrieved successfully",
                Data = result
            };
        }
    }
}