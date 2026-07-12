using MassTransit;
using NotificationsAPI.Application.Abstractions.Services;
using NotificationsAPI.Application.Services;
using NotificationsAPI.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddMassTransit(configuration =>
{
    configuration.AddConsumer<UserCreatedEventConsumer>();
    configuration.AddConsumer<PaymentProcessedEventConsumer>();

    configuration.UsingRabbitMq((context, rabbitMq) =>
    {
        rabbitMq.Host("localhost", "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        rabbitMq.ReceiveEndpoint(
            "notifications-user-created",
            endpoint =>
            {
                endpoint.ConfigureConsumer<UserCreatedEventConsumer>(
                    context);
            });

        rabbitMq.ReceiveEndpoint(
            "notifications-payment-processed",
            endpoint =>
            {
                endpoint.ConfigureConsumer<PaymentProcessedEventConsumer>(
                    context);
            });
    });
});

var app = builder.Build();

app.MapGet("/", () => "NotificationsAPI is running.");

app.Run();