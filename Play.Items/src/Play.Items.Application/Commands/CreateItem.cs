﻿using Play.Common.Abs.Commands;

namespace Play.Items.Application.Commands;

public record CreateItem(Guid ItemId, string Name, string Description, decimal Price) : ICommand
{
    //public Guid ItemId { get; } = Guid.NewGuid();
}