namespace NotificationsAPI.Application.Contracts.Events
{
    public class PaymentProcessedEvent
    {
        public Guid PaymentId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int GameId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}