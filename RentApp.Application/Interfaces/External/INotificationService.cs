namespace RentApp.Application.Interfaces.External
{
    public interface INotificationService
    {
        Task<NotificationResponseDto> CreateNotificationAsync(NotificationRequestDto request, CancellationToken cancellationToken = default);
        Task<NotificationResponseDto> GetNotificationById(Guid id, CancellationToken cancellationToken = default);
        Task<List<NotificationResponseDto>> GetNotificationsByUserId(Guid userId, CancellationToken cancellationToken = default);
        Task MarkAsReadAsync(Guid userId, Guid notificationId, CancellationToken cancellationToken = default);
        Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
