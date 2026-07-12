namespace NotificationsAPI.Shared.Logging
{
    public static class LogMessages
    {
        public static string WelcomeEmailSent(
            string userId,
            string email) =>
            $"Welcome email simulated successfully. " +
            $"UserId: {userId}, Email: {email}.";

        public static string PurchaseConfirmationSent(
            string userId,
            int gameId) =>
            $"Purchase confirmation simulated successfully. " +
            $"UserId: {userId}, GameId: {gameId}.";

        public static string NotificationError(Exception exception) =>
            $"Error processing notification. Details: {exception.Message}";
    }
}