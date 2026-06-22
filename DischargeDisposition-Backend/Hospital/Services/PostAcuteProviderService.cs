using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class PostAcuteProviderService
        : IPostAcuteProviderService
    {
        private readonly
            IPostAcuteProviderRepository
            _repository;

        public PostAcuteProviderService(
            IPostAcuteProviderRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<PostAcuteProviderResponse>> GetAllAsync()
        {
            var providers =
                await _repository.GetAllAsync();

            return providers.Select(provider =>
                new PostAcuteProviderResponse
                {
                    ProviderId =
                        provider.ProviderId,

                    ProviderName =
                        provider.ProviderName,

                    IsActive =
                        provider.IsActive,

                    Phone =
                        provider.Phone,

                    Email =
                        provider.Email,

                    ContactPerson =
                        provider.ContactPerson,

                    AddressLine =
                        provider.AddressLine,

                    City =
                        provider.City,

                    State =
                        provider.State,

                    DispositionTypeId =
                        provider.DispositionTypeId,

                    DispositionName =
                        provider.dispositionType
                            .DispositionName
                })
                .ToList();
        }

        public async Task<PostAcuteProviderResponse?>
            GetByIdAsync(int providerId)
        {
            var provider =
                await _repository
                    .GetByIdAsync(providerId);

            if (provider == null)
                return null;

            return new PostAcuteProviderResponse
            {
                ProviderId =
                    provider.ProviderId,

                ProviderName =
                    provider.ProviderName,

                IsActive =
                    provider.IsActive,

                Phone =
                    provider.Phone,

                Email =
                    provider.Email,

                ContactPerson =
                    provider.ContactPerson,

                AddressLine =
                    provider.AddressLine,

                City =
                    provider.City,

                State =
                    provider.State,

                DispositionTypeId =
                    provider.DispositionTypeId,

                DispositionName =
                    provider.dispositionType
                        .DispositionName
            };
        }

        public async Task<List<PostAcuteProviderResponse>>
            GetByDispositionTypeAsync(
                int dispositionTypeId)
        {
            var providers =
                await _repository
                    .GetByDispositionTypeAsync(
                        dispositionTypeId);

            return providers
                .Select(provider =>
                    new PostAcuteProviderResponse
                    {
                        ProviderId =
                            provider.ProviderId,

                        ProviderName =
                            provider.ProviderName,

                        IsActive =
                            provider.IsActive,

                        Phone =
                            provider.Phone,

                        Email =
                            provider.Email,

                        ContactPerson =
                            provider.ContactPerson,

                        AddressLine =
                            provider.AddressLine,

                        City =
                            provider.City,

                        State =
                            provider.State,

                        DispositionTypeId =
                            provider.DispositionTypeId,

                        DispositionName =
                            provider.dispositionType
                                .DispositionName
                    })
                .ToList();
        }
    }
}