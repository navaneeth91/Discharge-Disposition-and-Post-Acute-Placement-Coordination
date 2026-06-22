using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Insurance.Models;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class AuthorizationService
        : IAuthorizationService
    {
        private readonly IAuthorizationRepository _repository;

        private readonly InsuranceDbContext _insuranceContext;

        public AuthorizationService(
            IAuthorizationRepository repository,
            InsuranceDbContext insuranceContext)
        {
            _repository = repository;
            _insuranceContext = insuranceContext;
        }

        public async Task<long> CreateAsync(
            CreateAuthorizationRequest dto)
        {
            var externalId =
                $"AUTH-{Guid.NewGuid():N}"
                .Substring(0, 12);

            var tracking =
                new AuthorizationTracking
                {
                    PatientId = dto.PatientId,
                    ReferralId = dto.ReferralId,
                    PayerId = dto.PayerId,
                    ExternalAuthorizationId =
                        externalId,

                    Status =
                        AuthorizationStatus.Pending,

                    RequestedDate =
                        DateTime.UtcNow,

                    LastUpdated =
                        DateTime.UtcNow
                };

            await _repository.AddAsync(tracking);

            await _repository.SaveAsync();

            var request =
                new AuthorizationRequest
                {
                    MemberId =
                        dto.MemberId,

                    RequestingOrganization =
                        dto.RequestingOrganization,

                    ServiceType =
                        dto.ServiceType,

                    RequestedDate =
                        DateTime.UtcNow,

                    Status =
                        AuthorizationStatus.Pending
                };

            _insuranceContext.AuthorizationRequests
                .Add(request);

            await _insuranceContext
                .SaveChangesAsync();

            tracking.InsuranceAuthorizationRequestId =
                request.AuthorizationRequestId;

            await _repository.SaveAsync();

            return tracking.AuthorizationId;
        }

        public async Task<AuthorizationResponse?>
            GetAsync(long authorizationId)
        {
            var authorization =
                await _repository.GetAsync(
                    authorizationId);

            if (authorization == null)
                return null;

            return new AuthorizationResponse
            {
                AuthorizationId =
                    authorization.AuthorizationId,

                ExternalAuthorizationId =
                    authorization.ExternalAuthorizationId,

                Status =
                    authorization.Status,

                RequestedDate =
                    authorization.RequestedDate,

                ResponseDate =
                    authorization.ResponseDate,

                PayerName =
                    authorization.payer.PayerName,

                PatientName =
                    authorization.patient.FirstName
                    + " "
                    + authorization.patient.LastName,

                DenialReason =
                    authorization.DenialReason
            };
        }

        public async Task<List<AuthorizationResponse>>
            GetPatientAuthorizationsAsync(
                int patientId)
        {
            var authorizations =
                await _repository
                    .GetByPatientAsync(patientId);

            return authorizations
                .Select(a =>
                    new AuthorizationResponse
                    {
                        AuthorizationId =
                            a.AuthorizationId,

                        ExternalAuthorizationId =
                            a.ExternalAuthorizationId,

                        Status =
                            a.Status,

                        RequestedDate =
                            a.RequestedDate,

                        ResponseDate =
                            a.ResponseDate,

                        PayerName =
                            a.payer.PayerName,

                        PatientName =
                            a.patient.FirstName
                            + " "
                            + a.patient.LastName,

                        DenialReason =
                            a.DenialReason
                    })
                .ToList();
        }

        public async Task ProcessWebhookAsync(
            AuthorizationWebhook dto)
        {
            var authorization =
                await _repository
                    .GetByInsuranceRequestIdAsync(
                        dto.AuthorizationRequestId);

            if (authorization == null)
            {
                throw new KeyNotFoundException(
                    "Authorization not found.");
            }

            if (authorization.Status ==
                dto.Status)
            {
                return;
            }

            authorization.Status =
                dto.Status;

            authorization.ResponseDate =
                dto.DecisionDate;

            authorization.DenialReason =
                dto.ReasonCode;

            authorization.LastUpdated =
                DateTime.UtcNow;

            await _repository.SaveAsync();
        }
    }
}