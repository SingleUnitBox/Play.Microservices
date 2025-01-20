using Play.Common.Exceptions.Mappers;

namespace Play.Items.Infra.Exceptions;

public class ExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
        => exception switch
        {
            // ItemAlreadyExistException ex =>
            //     new CreateItemRejected(ex.ItemId, ex.Message, ex.Message, ex.GetType().Name),
            // ItemNotFoundException ex => message switch
            // {
            //     UpdateItem cmd => new UpdateItemRejected(cmd.ItemId, ex.Message, ex.GetType().Name),
            //     DeleteItem cmd => new DeleteItemRejected(cmd.ItemId, ex.Message, ex.GetType().Name),
            //     _ => null
            // },
            // _ => null
        };
}