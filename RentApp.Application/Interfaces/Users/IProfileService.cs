using RentApp.Application.DTOs.Users;

namespace RentApp.Application.Interfaces.Users
{
    internal interface IProfileService
    {
        Task<ProfileDto?> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<ProfileDto> UpdateProfileAsync(Guid userId,
            UpdateProfileDto dto,
            CancellationToken cancellationToken = default);
    }
}
