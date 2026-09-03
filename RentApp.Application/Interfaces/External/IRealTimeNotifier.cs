namespace RentApp.Application.Interfaces.External
{
    public interface IRealTimeNotifier
    {
        Task SendToUserAsync(string userId, string message, CancellationToken ct = default);
        Task SendToAllAsync(string message, CancellationToken ct = default);
    }
}
