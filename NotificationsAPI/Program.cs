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
        var host = builder.Configuration["RabbitMq:Host"] ?? "localhost";
        var username = builder.Configuration["RabbitMq:Username"] ?? "guest";
        var password = builder.Configuration["RabbitMq:Password"] ?? "guest";

        rabbitMq.Host(host, "/", hostConfiguration =>
        {
            hostConfiguration.Username(username);
            hostConfiguration.Password(password);
        });

        rabbitMq.ReceiveEndpoint(
            builder.Configuration["RabbitMq:UserCreatedQueue"]
                ?? "user-created-notifications",
            endpoint =>
            {
                endpoint.ConfigureConsumer<UserCreatedEventConsumer>(context);
            });

        rabbitMq.ReceiveEndpoint(
            builder.Configuration["RabbitMq:PaymentProcessedQueue"]
                ?? "payment-processed-notifications",
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