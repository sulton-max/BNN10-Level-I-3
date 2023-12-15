using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.Caching.Brokers;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories;

public class UserInfoVerificationCodeRepository(IdentityDbContext identityDbContext, ICacheBroker cacheBroker)
    : EntityRepositoryBase<UserInfoVerificationCode, IdentityDbContext>(identityDbContext, cacheBroker), IUserInfoVerificationCodeRepository
{
    public new IQueryable<UserInfoVerificationCode> Get(
        Expression<Func<UserInfoVerificationCode, bool>>? predicate = default,
        bool asNoTracking = false
    )
    {
        return base.Get(predicate, asNoTracking);
    }

    public new ValueTask<UserInfoVerificationCode?> GetByIdAsync(
        Guid codeId,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return base.GetByIdAsync(codeId, asNoTracking, cancellationToken);
    }

    public new async ValueTask<UserInfoVerificationCode> CreateAsync(
        UserInfoVerificationCode verificationCode,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    )
    {
        await DbContext.UserInfoVerificationCodes.Where(code => code.UserId == verificationCode.UserId && code.CodeType == verificationCode.CodeType)
            .ExecuteUpdateAsync(setter => setter.SetProperty(code => code.IsActive, false), cancellationToken);

        return await base.CreateAsync(verificationCode, saveChanges, cancellationToken);
    }

    public async ValueTask DeactivateAsync(Guid codeId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await DbContext.UserInfoVerificationCodes.Where(code => code.Id == codeId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(code => code.IsActive, false), cancellationToken);

        if (saveChanges)
            await DbContext.SaveChangesAsync(cancellationToken);
    }
}