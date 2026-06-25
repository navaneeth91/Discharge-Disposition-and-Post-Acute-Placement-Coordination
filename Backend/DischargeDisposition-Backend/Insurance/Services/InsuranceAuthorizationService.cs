using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Insurance.Models;
using DischargeDisposition_Backend.Insurance.Repositories.Interfaces;
using DischargeDisposition_Backend.Insurance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Insurance.Services
{
    public class InsuranceAuthorizationService : IInsuranceAuthorizationService
    {
        private readonly IInsuranceAuthorizationRepository _repository;
        private readonly InsuranceDbContext _context;
        private readonly IWebhookService _webhookService;

        public InsuranceAuthorizationService(
            IInsuranceAuthorizationRepository repository,
            InsuranceDbContext context,
            IWebhookService webhookService)
        {
            _repository = repository;
            _context = context;
            _webhookService = webhookService;
        }

        public async Task<ApiResponse<PagedResponse<AuthorizationRequestListItemResponse>>> GetAuthorizationsAsync(
            string? search,
            AuthorizationStatus? status,
            int page,
            int pageSize)
        {
            try
            {
                page = page < 1 ? 1 : page;
                pageSize = pageSize < 1 ? 10 : pageSize;

                var (items, totalCount) = await _repository.GetAuthorizationsAsync(search, status, page, pageSize);

                var data = items.Select(MapAuthorization).ToList();

                return new ApiResponse<PagedResponse<AuthorizationRequestListItemResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Authorizations retrieved successfully",
                    Data = new PagedResponse<AuthorizationRequestListItemResponse>
                    {
                        Items = data,
                        TotalCount = totalCount,
                        Page = page,
                        PageSize = pageSize
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PagedResponse<AuthorizationRequestListItemResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve authorizations",
                    Errors = new() { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<List<AuthorizationRequestListItemResponse>>> GetRecentAuthorizationsAsync(int take)
        {
            try
            {
                take = take < 1 ? 5 : take;

                var recent = await _context.AuthorizationRequests
                    .AsNoTracking()
                    .Include(x => x.member)
                    .Include(x => x.AuthorizationDecisions)
                    .OrderByDescending(x => x.RequestedDate)
                    .Take(take)
                    .ToListAsync();

                return new ApiResponse<List<AuthorizationRequestListItemResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Recent authorizations retrieved successfully",
                    Data = recent.Select(MapAuthorization).ToList()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AuthorizationRequestListItemResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve recent authorizations",
                    Errors = new() { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<AuthorizationRequestListItemResponse>> GetAuthorizationAsync(int authorizationRequestId)
        {
            try
            {
                var authorization = await _repository.GetByIdAsync(authorizationRequestId);

                if (authorization == null)
                {
                    return new ApiResponse<AuthorizationRequestListItemResponse>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "Authorization request not found"
                    };
                }

                return new ApiResponse<AuthorizationRequestListItemResponse>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Authorization retrieved successfully",
                    Data = MapAuthorization(authorization)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthorizationRequestListItemResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve authorization",
                    Errors = new() { ex.Message }
                };
            }
        }

        public async Task UpdateStatusAsync(int authorizationRequestId, UpdateAuthorizationStatus dto)
        {
            var request = await _repository.GetByIdWithTrackingAsync(authorizationRequestId);

            if (request == null)
            {
                throw new KeyNotFoundException("Authorization request not found.");
            }

            request.Status = dto.Status;

            var decision = new AuthorizationDecision
            {
                AuthorizationRequestId = authorizationRequestId,
                DecisionStatus = dto.Status,
                DecisionDate = DateTime.UtcNow,
                ReasonCode = dto.ReasonCode,
                Notes = dto.Notes
            };

            _context.AuthorizationDecisions.Add(decision);
            await _context.SaveChangesAsync();

            var webhook = new AuthorizationWebhook
            {
                AuthorizationRequestId = authorizationRequestId,
                Status = dto.Status,
                DecisionDate = DateTime.UtcNow,
                ReasonCode = dto.ReasonCode,
                Notes = dto.Notes
            };

            await _webhookService.SendAuthorizationUpdateAsync(webhook);
        }

        private static AuthorizationRequestListItemResponse MapAuthorization(AuthorizationRequest authorization)
        {
            var latestDecision = authorization.AuthorizationDecisions
                .OrderByDescending(d => d.DecisionDate)
                .FirstOrDefault();

            return new AuthorizationRequestListItemResponse
            {
                AuthorizationRequestId = authorization.AuthorizationRequestId,
                MemberId = authorization.MemberId,
                MemberName = authorization.member.FirstName + " " + authorization.member.LastName,
                PolicyNumber = authorization.member.PolicyNumber,
                RequestingOrganization = authorization.RequestingOrganization,
                ServiceType = authorization.ServiceType,
                RequestedDate = authorization.RequestedDate,
                Status = authorization.Status,
                LatestDecisionDate = latestDecision?.DecisionDate,
                ReasonCode = latestDecision?.ReasonCode,
                Notes = latestDecision?.Notes
            };
        }
    }
}
