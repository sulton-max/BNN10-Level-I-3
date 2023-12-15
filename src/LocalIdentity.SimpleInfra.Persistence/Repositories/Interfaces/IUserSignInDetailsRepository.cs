using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

public interface IUserSignInDetailsRepository
{
    IQueryable<UserSignInDetails> Get(Expression<Func<UserSignInDetails, bool>>? predicate = default, bool asNoTracking = false);

    ValueTask<UserSignInDetails> CreateAsync(UserSignInDetails userSignInDetails, bool saveChanges = true, CancellationToken cancellationToken = default);
}