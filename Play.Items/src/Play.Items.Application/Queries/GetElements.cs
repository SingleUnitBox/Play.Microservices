using Play.Common.Abs.Queries;
using Play.Items.Application.DTO;

namespace Play.Items.Application.Queries;

public record GetElements : IQuery<IEnumerable<ElementDto>>;