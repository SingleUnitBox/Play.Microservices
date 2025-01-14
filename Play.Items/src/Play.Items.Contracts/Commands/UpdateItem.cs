﻿using Play.Common.Abs.Commands;

namespace Play.Items.Contracts.Commands;

public record UpdateItem(Guid ItemId, string Name, string Description, decimal Price) : ICommand;