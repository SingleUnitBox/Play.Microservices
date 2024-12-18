using System.Linq.Expressions;

namespace Play.User.Core.Repositories;

public interface IUserRepository
{
    Task CreateUser(Entities.User user);
    Task UpdateUser(Entities.User user);
    Task<Entities.User> GetUserById(Guid userId);
    Task<Entities.User> GetUser(Expression<Func<Entities.User, bool>> filter);
}