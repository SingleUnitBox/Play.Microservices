using Play.Items.Domain.Entities;

namespace Play.Items.Domain.Repositories;

public interface ICrafterRepository
{
    Task<Crafter> GetCrafterById(Guid crafterId);
    Task AddCrafter(Crafter crafter);
}