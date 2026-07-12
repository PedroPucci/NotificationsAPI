using MassTransit;
using NotificationsAPI.Application.Abstractions.Services;
using NotificationsAPI.Application.Contracts.Events;

namespace NotificationsAPI.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly INotificationService _notificationService;

        public UserCreatedEventConsumer(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(
            ConsumeContext<UserCreatedEvent> context)
        {
            var message = context.Message;

            await _notificationService.SendWelcomeEmail(
                message.UserId,
                message.Email,
                message.Name);
        }
    }
}