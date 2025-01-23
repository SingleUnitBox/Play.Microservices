﻿using Play.Common.Abs.Exceptions;
using Play.Items.Application.Commands;
using Play.Items.Application.Events.RejectedEvents;
using Play.Items.Application.Exceptions;

namespace Play.Items.Infra.Exceptions;

public class ExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
        => exception switch
        {
            ItemAlreadyExistException ex =>
                new CreateItemRejected(ex.ItemId, ex.Message, ex.Message, ex.GetType().Name),
            ItemNotFoundException ex => message switch
            {
                UpdateItem cmd => new UpdateItemRejected(cmd.ItemId, ex.Message, ex.GetType().Name),
                DeleteItem cmd => new DeleteItemRejected(cmd.ItemId, ex.Message, ex.GetType().Name),
                _ => null
            },
            _ => null
        };
}