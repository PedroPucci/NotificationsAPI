using MassTransit;
using NotificationsAPI.Application.Abstractions.Services;
using NotificationsAPI.Application.Contracts.Events;

namespace NotificationsAPI.Consumers
{
    public class PaymentProcessedEventConsumer
        : IConsumer<PaymentProcessedEvent>
    {
        private readonly INotificationService _notificationService;

        public PaymentProcessedEventConsumer(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(
            ConsumeContext<PaymentProcessedEvent> context)
        {
            var message = context.Message;

            if (!string.Equals(
                    message.Status,
                    "Approved",
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            await _notificationService.SendPurchaseConfirmationEmail(
                message.UserId,
                message.Email,
                message.GameId,
                message.Price);
        }
    }
}