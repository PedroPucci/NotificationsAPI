namespace NotificationsAPI.Application.Abstractions.Services
{
    public interface INotificationService
    {
        Task SendWelcomeEmail(
            string userId,
            string email,
            string name);

        Task SendPurchaseConfirmationEmail(
            string userId,
            string email,
            int gameId,
            decimal price);
    }
}