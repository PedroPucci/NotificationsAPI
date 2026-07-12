using NotificationsAPI.Application.Abstractions.Services;
using Serilog;

namespace NotificationsAPI.Application.Services
{
    public class NotificationService : INotificationService
    {
        public Task SendWelcomeEmail(
            string userId,
            string email,
            string name)
        {
            Log.Information(
                "Welcome email sent. UserId: {UserId}, Email: {Email}, Name: {Name}",
                userId,
                email,
                name);

            Console.WriteLine(
                $"Welcome email sent to {email}. Welcome, {name}!");

            return Task.CompletedTask;
        }

        public Task SendPurchaseConfirmationEmail(
            string userId,
            string email,
            int gameId,
            decimal price)
        {
            Log.Information(
                "Purchase confirmation email sent. UserId: {UserId}, Email: {Email}, GameId: {GameId}, Price: {Price}",
                userId,
                email,
                gameId,
                price);

            Console.WriteLine(
                $"Purchase confirmation email sent to {email}. GameId: {gameId}, Price: {price:C}");

            return Task.CompletedTask;
        }
    }
}