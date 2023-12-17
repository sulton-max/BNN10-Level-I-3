using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N75_C.Models.Common;
using N75_C.Models.Entities;
using N75_C.Models.Enums;

namespace N75_C.EntityConfigurations;

public class NotificationEventConfiguration : IEntityTypeConfiguration<NotificationEvent>
{
    public void Configure(EntityTypeBuilder<NotificationEvent> builder)
    {
        builder.ToTable("NotificationEvents")
            .HasDiscriminator(notificationEvent => notificationEvent.Type)
            .HasValue<EmailNotificationEvent>(NotificationType.Email);
    }
}