using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.Caching.Brokers;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories;

public class EmailTemplateRepository(NotificationDbContext dbContext, ICacheBroker cacheBroker)
    : EntityRepositoryBase<EmailTemplate, NotificationDbContext>(dbContext, cacheBroker), IEmailTemplateRepository
{
    public new IQueryable<EmailTemplate> Get(Expression<Func<EmailTemplate, bool>>? predicate = default, bool asNoTracking = false)
    {
        return base.Get(predicate, asNoTracking);
    }

    public new ValueTask<EmailTemplate> CreateAsync(
        EmailTemplate emailTemplate,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    )
    {
        return base.CreateAsync(emailTemplate, saveChanges, cancellationToken);
    }
}