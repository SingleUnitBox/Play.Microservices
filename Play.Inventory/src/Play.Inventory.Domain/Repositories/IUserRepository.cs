using Play.Common.Abs.SharedKernel.Types;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid userId);
    Task<User> GetByIdAsync(Guid userId);
}