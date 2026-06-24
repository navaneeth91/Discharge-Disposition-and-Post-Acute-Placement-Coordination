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

        public async Task<
            ApiResponse<List<PostAcuteProviderResponse>>>
            GetAllAsync()
        {
            try
            {
                var providers =
                    await _repository.GetAllAsync();

                var result =
                    providers.Select(provider =>
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

                return new ApiResponse<
                    List<PostAcuteProviderResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Providers retrieved successfully",

                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<PostAcuteProviderResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve providers",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<
            ApiResponse<PostAcuteProviderResponse>>
            GetByIdAsync(
                int providerId)
        {
            try
            {
                var provider =
                    await _repository
                        .GetByIdAsync(providerId);

                if (provider == null)
                {
                    return new ApiResponse<
                        PostAcuteProviderResponse>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Provider not found"
                    };
                }

                return new ApiResponse<
                    PostAcuteProviderResponse>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Provider retrieved successfully",

                    Data =
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
                        }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    PostAcuteProviderResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve provider",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<
            ApiResponse<List<PostAcuteProviderResponse>>>
            GetByDispositionTypeAsync(
                int dispositionTypeId)
        {
            try
            {
                var providers =
                    await _repository
                        .GetByDispositionTypeAsync(
                            dispositionTypeId);

                var result =
                    providers.Select(provider =>
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

                return new ApiResponse<
                    List<PostAcuteProviderResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Providers retrieved successfully",

                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<PostAcuteProviderResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve providers",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}