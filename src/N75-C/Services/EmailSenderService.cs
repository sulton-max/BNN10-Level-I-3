using System.Collections.Immutable;
using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using N75_C.DataContexts;
using N75_C.Models.Common;
using N75_C.Models.Entities;
using N75_C.Models.Settings;

namespace N75_C.Services;

public class EmailSenderService(
    IOptions<SmtpEmailSenderSettings> smtpEmailSenderSettings,
    IOptions<NotificationSenderSettings> notificationSenderSettings,
    IdentityDbContext identityDbContext
)
{
    private readonly SmtpEmailSenderSettings _smtpEmailSenderSettings = smtpEmailSenderSettings.Value;
    private readonly NotificationSenderSettings _notificationSenderSettings = notificationSenderSettings.Value;

    public async ValueTask<bool> UpdateEventAsync(
        NotificationEvent notificationEvent,
        CancellationToken cancellationToken = default
    )
    {
        identityDbContext.Update(notificationEvent);
        return await identityDbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async ValueTask<List<NotificationEvent>> GetFailedEventsAsync(int batchSize)
    {
        var failedNotificationEvents = new List<NotificationEvent>();

        // query failed email events
        failedNotificationEvents.AddRange(await identityDbContext.EmailNotificationEvents
            .Where(emailNotificationEvent => !emailNotificationEvent.IsCancelled &&
                                             !emailNotificationEvent.IsSuccessful &&
                                             emailNotificationEvent.ResentAttempts <=
                                             _notificationSenderSettings.ResendAttemptsThreshold)
            .OrderBy(emailNotificationEvent => emailNotificationEvent.CreatedAt)
            .ToListAsync());

        // query failed sms events

        return failedNotificationEvents.Take(batchSize).ToList();
    }

    public async ValueTask<bool> QueueAsync(
        Guid receiverUserId,
        string emailAddress,
        string subject,
        string body,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    )
    {
        var emailNotificationEvent = new EmailNotificationEvent
        {
            ReceiverUserId = receiverUserId,
            ReceiverEmailAddress = emailAddress,
            Subject = subject,
            Content = body,
            CreatedAt = DateTime.UtcNow
        };

        identityDbContext.Add(emailNotificationEvent);

        return !saveChanges || await identityDbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public ValueTask<bool> SendAsync(
        string emailAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    )
    {
        var mail = new MailMessage(_smtpEmailSenderSettings.CredentialAddress, emailAddress);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        var smtpClient = new SmtpClient(_smtpEmailSenderSettings.Host, _smtpEmailSenderSettings.Port);
        smtpClient.Credentials =
            new NetworkCredential(_smtpEmailSenderSettings.CredentialAddress, _smtpEmailSenderSettings.Password);
        smtpClient.EnableSsl = true;

        smtpClient.Send(mail);

        return new ValueTask<bool>(true);
    }
}