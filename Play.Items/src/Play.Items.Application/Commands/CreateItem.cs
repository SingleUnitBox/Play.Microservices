﻿using Play.Common.Abs.Commands;

namespace Play.Items.Application.Commands;

public record CreateItem(string Name, string Description, decimal Price) : ICommand
{
    public Guid ItemId { get; } = Guid.NewGuid();
}