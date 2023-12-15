using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

public interface IRoleRepository
{
    IQueryable<Role> Get(Expression<Func<Role, bool>>? predicate = default, bool asNoTracking = false);
}