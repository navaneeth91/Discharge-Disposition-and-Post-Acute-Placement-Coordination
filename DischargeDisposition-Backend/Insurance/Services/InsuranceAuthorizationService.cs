using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Insurance.Models;
using DischargeDisposition_Backend.Insurance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Insurance.Services
{
    public class InsuranceAuthorizationService
        : IInsuranceAuthorizationService
    {
        private readonly InsuranceDbContext _context;

        private readonly IWebhookService _webhookService;

        public InsuranceAuthorizationService(
            InsuranceDbContext context,
            IWebhookService webhookService)
        {
            _context = context;
            _webhookService = webhookService;
        }

        public async Task UpdateStatusAsync(
            int authorizationRequestId,
            UpdateAuthorizationStatus dto)
        {
            var request =
                await _context.AuthorizationRequests
                    .FirstOrDefaultAsync(x =>
                        x.AuthorizationRequestId ==
                        authorizationRequestId);

            if (request == null)
            {
                throw new KeyNotFoundException(
                    "Authorization request not found.");
            }

            request.Status = dto.Status;

            var decision =
                new AuthorizationDecision
                {
                    AuthorizationRequestId =
                        authorizationRequestId,

                    DecisionStatus =
                        dto.Status,

                    DecisionDate =
                        DateTime.UtcNow,

                    ReasonCode =
                        dto.ReasonCode,

                    Notes =
                        dto.Notes
                };

            _context.AuthorizationDecisions
                .Add(decision);

            await _context.SaveChangesAsync();

            var webhook =
                new AuthorizationWebhook
                {
                    AuthorizationRequestId =
                        authorizationRequestId,

                    Status =
                        dto.Status,

                    DecisionDate =
                        DateTime.UtcNow,

                    ReasonCode =
                        dto.ReasonCode,

                    Notes =
                        dto.Notes
                };

            await _webhookService
                .SendAuthorizationUpdateAsync(
                    webhook);
        }
    }
}