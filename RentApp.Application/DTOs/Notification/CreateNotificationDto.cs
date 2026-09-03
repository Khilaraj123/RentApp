namespace RentApp.Application.DTOs.Notification
{
    public record CreateNotificationDto
    {
        public string Title { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
    }
}
