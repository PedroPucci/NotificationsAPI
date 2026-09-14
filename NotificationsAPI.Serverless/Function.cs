using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using FCG.Contracts.Events;
using NotificationsAPI.Application.Services;

[assembly: LambdaSerializer(
    typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace NotificationsAPI.Serverless;

public class Function
{
    private readonly NotificationService _notificationService;

    public Function()
    {
        _notificationService = new NotificationService();
    }

    public async Task FunctionHandler(
        SQSEvent evnt,
        ILambdaContext context)
    {
        foreach (var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(
        SQSEvent.SQSMessage message,
        ILambdaContext context)
    {
        context.Logger.LogInformation(
            $"Processing notification message: {message.MessageId}");

        using var document = JsonDocument.Parse(message.Body);

        if (!document.RootElement.TryGetProperty(
                "eventType",
                out var eventTypeProperty))
        {
            throw new InvalidOperationException(
                "Message does not contain eventType.");
        }

        var eventType = eventTypeProperty.GetString();

        switch (eventType)
        {
            case "UserCreated":
                await ProcessUserCreatedAsync(
                    document.RootElement,
                    context);
                break;

            case "PaymentProcessed":
                await ProcessPaymentProcessedAsync(
                    document.RootElement,
                    context);
                break;

            default:
                throw new InvalidOperationException(
                    $"Unsupported event type: {eventType}");
        }
    }

    private async Task ProcessUserCreatedAsync(
        JsonElement root,
        ILambdaContext context)
    {
        var message =
            root.GetProperty("data")
                .Deserialize<UserCreatedEvent>();

        if (message is null)
        {
            throw new InvalidOperationException(
                "Invalid UserCreatedEvent.");
        }

        await _notificationService.SendWelcomeEmail(
            message.UserId,
            message.Email,
            message.Name);

        context.Logger.LogInformation(
            $"Welcome notification processed for UserId: {message.UserId}");
    }

    private async Task ProcessPaymentProcessedAsync(
        JsonElement root,
        ILambdaContext context)
    {
        var message =
            root.GetProperty("data")
                .Deserialize<PaymentProcessedEvent>();

        if (message is null)
        {
            throw new InvalidOperationException(
                "Invalid PaymentProcessedEvent.");
        }

        if (!string.Equals(
                message.Status,
                "Approved",
                StringComparison.OrdinalIgnoreCase))
        {
            context.Logger.LogInformation(
                $"Payment {message.Status}. Notification ignored.");

            return;
        }

        await _notificationService.SendPurchaseConfirmationEmail(
            message.UserId,
            message.Email,
            message.GameId,
            message.Price);

        context.Logger.LogInformation(
            $"Purchase notification processed for UserId: {message.UserId}");
    }
}