using System.Runtime.InteropServices.JavaScript;
using NSubstitute;
using Play.Common.Abs.Commands;
using Play.Common.Abs.RabbitMq;
using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Application.Commands;
using Play.Items.Application.Commands.Handlers;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;
using Shouldly;
using Xunit;

namespace Play.Items.Tests.Unit.Application.Commands.Handlers;

public class DeleteItemHandlerTests
{
    private Task Act(DeleteItem command) => _commandHandler.HandleAsync(command);

    [Fact]
    public async Task delete_item_handler_should_throw_item_not_found_exception_when_item_not_found()
    {
        _itemRepository.GetByIdAsync(Arg.Any<AggregateRootId>()).Returns(Task.FromResult<Item>(null));
        var command = new DeleteItem(Guid.NewGuid());
        
        var exception = await Record.ExceptionAsync(() => Act(command));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ItemNotFoundException>();
    }

    [Fact]
    public async Task delete_item_handler_should_delete_existing_item_from_repository()
    {
        var item = GetItem();
        _itemRepository.GetByIdAsync(item.Id).Returns(Task.FromResult(item));
        var command = new DeleteItem(item.Id);

        await Act(command);
        
        _itemRepository.Received(1).DeleteAsync(item.Id);
        var items = await _itemRepository.GetAllAsync();
        items.ShouldBeEmpty();
    }

    private readonly IItemRepository _itemRepository;
    private readonly IBusPublisher _busPublisher;
    private readonly ICommandHandler<DeleteItem> _commandHandler;

    public DeleteItemHandlerTests()
    {
        _itemRepository = Substitute.For<IItemRepository>();
        _busPublisher = Substitute.For<IBusPublisher>();
        _commandHandler = new DeleteItemHandler(_itemRepository, _busPublisher);
    }

    private Item GetItem()
        => Item.Create("Sword", "Deals a lot of damage", 20.30m, DateTimeOffset.Now);
}