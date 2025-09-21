using Play.Common.Abs.Exceptions;
using Play.Items.Application.Commands;
using Play.Items.Application.Events.RejectedEvents;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Exceptions;

namespace Play.Items.Infra.Exceptions;

internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
        => exception switch
        {
            ItemAlreadyExistException ex =>
                new CreateItemRejected(ex.ItemId, ((CreateItem)message).Name, ex.Message, ex.GetType().Name),
            ItemNotFoundException ex => message switch
            {
                UpdateItem cmd => new UpdateItemRejected(cmd.ItemId, ex.Message, ex.GetType().Name),
                DeleteItem cmd => new DeleteItemRejected(cmd.ItemId, ex.Message, ex.GetType().Name),
                _ => null
            },
            EmptyNameException ex => message switch
            {
                CreateItem cmd => new CreateItemRejected(cmd.ItemId, cmd.Name, ex.Message, ex.GetType().Name),
                UpdateItem cmd => new UpdateItemRejected(cmd.ItemId, ex.Message, ex.GetType().Name),
                _ => null
            },
            EmptyDescriptionException ex => message switch
            {
                CreateItem cmd => new CreateItemRejected(cmd.ItemId, cmd.Name, ex.Message, ex.GetType().Name),
                UpdateItem cmd => new UpdateItemRejected(cmd.ItemId, ex.Message, ex.GetType().Name),
                _ => null
            },
            InvalidPriceException ex => message switch
            {
                CreateItem cmd => new CreateItemRejected(cmd.ItemId, cmd.Name, ex.Message, ex.GetType().Name),
                UpdateItem cmd => new UpdateItemRejected(cmd.ItemId, ex.Message, ex.GetType().Name),
                _ => null
            },
            CrafterNotFoundException ex => message switch
            {
                CreateItem cmd => new CreateItemRejected(cmd.ItemId, cmd.Name, ex.Message, ex.GetType().Name),
                UpdateItem cmd => new UpdateItemRejected(cmd.ItemId, ex.Message, ex.GetType().Name),
                _ => null
            },
            _ => null
        };
}