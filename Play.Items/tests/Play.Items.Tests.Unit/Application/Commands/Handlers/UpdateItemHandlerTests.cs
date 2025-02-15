using NSubstitute;
using Play.Common.Abs.Commands;
using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Application.Commands;
using Play.Items.Application.Commands.Handlers;
using Play.Items.Application.Exceptions;
using Play.Items.Application.Services;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;
using Shouldly;
using Xunit;

namespace Play.Items.Tests.Unit.Application.Commands.Handlers;

public class UpdateItemHandlerTests
{
    private Task Act(UpdateItem command) => _commandHandler.HandleAsync(command);

    [Fact]
    public async Task update_item_handler_should_throw_item_not_found_exception_when_item_not_found()
    {
        _itemRepository.GetByIdAsync(Arg.Any<AggregateRootId>()).Returns(Task.FromResult<Item>(null));
        var command = new UpdateItem(Guid.NewGuid(), "Sword", "Heavy metal object", 20.30m);

        var exception = await Record.ExceptionAsync(() => Act(command));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ItemNotFoundException>();
    }

    [Fact]
    public async Task update_item_handler_should_update_name_description_and_price_when_command_input_is_valid()
    {
        var item = GetItem();
        _itemRepository.GetByIdAsync(item.Id).Returns(Task.FromResult(item));
        var command = new UpdateItem(item.Id, "Sword", "Heavy metal object", 19.99m);

        await Act(command);

        var updatedItem = await _itemRepository.GetByIdAsync(item.Id);
        updatedItem.Name.Value.ShouldBe(command.Name);
        updatedItem.Description.Value.ShouldBe(command.Description);
        updatedItem.Price.Value.ShouldBe(command.Price);
    }

    private readonly IItemRepository _itemRepository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICommandHandler<UpdateItem> _commandHandler;

    public UpdateItemHandlerTests()
    {
        _itemRepository = Substitute.For<IItemRepository>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _commandHandler = new UpdateItemHandler(_itemRepository, _eventProcessor);
    }

    private Item GetItem()
        => Item.Create("Sword", "Deals a lot of damage", 20.30m, DateTimeOffset.Now);
}