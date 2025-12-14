using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Domain.Entities;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Repositories;

public interface ISocketRepository
{
    Task<Socket> GetByItemIdAsync(AggregateRootId itemId);
}