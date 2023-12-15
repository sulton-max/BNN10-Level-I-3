using System.Collections.Immutable;
using Microsoft.Extensions.Options;
using N75_C.Extensions;
using N75_C.Models.Common;
using N75_C.Models.Entities;
using N75_C.Models.Settings;
using Polly;

namespace N75_C.Services;

public class EmailSenderBackgroundService(
    IOptions<NotificationSenderSettings> notificationSenderSettings,
    EmailSenderService emailSenderService
) : BackgroundService
{
    private readonly NotificationSenderSettings _notificationSenderSettings = notificationSenderSettings.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Started background service");

        while (!stoppingToken.IsCancellationRequested)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(_notificationSenderSettings.ResendAttemptsThreshold,
                    _ => _notificationSenderSettings.ResendIntervalInSeconds > 0
                        ? TimeSpan.FromSeconds(_notificationSenderSettings.ResendAttemptsThreshold)
                        : TimeSpan.Zero);

            var failedNotificationEvents =
                await emailSenderService.GetFailedEventsAsync(_notificationSenderSettings.BatchSize);
            await retryPolicy.ExecuteAsync(async () =>
                await ProcessFailedEventsAsync(failedNotificationEvents, stoppingToken));

            await Task.Delay(_notificationSenderSettings.BatchResentIntervalInSeconds, stoppingToken);
        }

        Console.WriteLine("Stopped background service");
    }

    private async ValueTask ProcessFailedEventsAsync(
        List<NotificationEvent> notificationEvents,
        CancellationToken cancellationToken = default
    )
    {
        var exception = default(Exception?);

        var notificationResults = notificationEvents.Select(async (notificationEvent) =>
        {
            if (notificationEvent is EmailNotificationEvent emailNotificationEvent)
            {
                var sendNotificationTask = async () =>
                    await emailSenderService.SendAsync(emailNotificationEvent.ReceiverEmailAddress,
                        emailNotificationEvent.Subject,
                        emailNotificationEvent.Content,
                        cancellationToken: cancellationToken);

                var sendNotificationResult = await sendNotificationTask.GetValueAsync();

                emailNotificationEvent.IsSuccessful = sendNotificationResult.IsSuccess;
                emailNotificationEvent.ResentAttempts++;

                var updateNotificationTask = async () =>
                    await emailSenderService.UpdateEventAsync(emailNotificationEvent, cancellationToken);

                await updateNotificationTask.GetValueAsync();

                if (sendNotificationResult is { IsSuccess: false, Exception: not null })
                    exception = sendNotificationResult.Exception;
            }
        });

        await Task.WhenAll(notificationResults);
        notificationEvents.RemoveAll(result => result.IsSuccessful);

        if (exception is not null)
            throw exception;
    }
}