using Play.Common.Abs.Queries;
using Play.Items.Application.DTO;

namespace Play.Items.Application.Queries;

public record GetCrafter(Guid CrafterId) : IQuery<CrafterDto>;