﻿using Play.Common.Abs.Commands;

namespace Play.Items.Application.Commands;

public record DeleteItem(Guid ItemId) : ICommand;