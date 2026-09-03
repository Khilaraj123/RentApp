using RentApp.Application.DTOs.Notification;
using RentApp.Application.DTOs.Notifications;

namespace RentApp.Application.Interfaces.External
{
    public interface INotificationService
    {
        Task SendNotificationToUserAsync(Guid userId, string message, CancellationToken cancellationToken = default);
        Task SendNotificationToAllAsync(string message, CancellationToken cancellationToken = default);
        Task CreateNotificationAsync(CreateNotificationDto dto, CancellationToken cancellationToken = default);
        Task<List<NotificationDto>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default);
        Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
