using NSubstitute;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using Play.Items.Application.Commands;
using Play.Items.Application.Commands.Handlers;
using Play.Items.Application.Exceptions;
using Play.Items.Application.Services;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;
using Play.Items.Domain.Types;
using Shouldly;
using Xunit;

namespace Play.Items.Tests.Unit.Application.Commands.Handlers;

public class CreateItemHandlerTests
{
    private Task Act(CreateItem command) => _commandHandler.HandleAsync(command);

    [Fact]
    public async Task create_item_handler_should_throw_item_already_exists_exception_when_item_already_exists()
    {
        var item = GetItem();
        _itemRepository.GetByIdAsync(item.Id).Returns(item);
        var command = new CreateItem("Sword", "Deals a lot of damage", 20.30m, Guid.NewGuid(), "Fire")
        {
            ItemId = item.Id,
        };
        
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ItemAlreadyExistException>();
    }

    [Fact]
    public async Task create_item_handler_should_add_new_item_and_call_item_repository()
    {
        var item = GetItem();
        var crafter = GetCrafter();
        var element = GetElement();
        item.SetCrafter(crafter);
        item.SetElement(element);
        
        _crafterRepository.GetCrafterById(crafter.CrafterId).Returns(crafter);
        _elementRepository.GetElement(Elements.Fire.ToString()).Returns(element);
        
        var command = new CreateItem("Sword", "Deals a lot of damage", 20.30m, crafter.CrafterId,
            element.ElementName.Value)
        {
            ItemId = item.Id,
        };

        await Act(command);

        await _itemRepository.Received(1).CreateAsync(Arg.Is<Item>(i => i.Id.Value == item.Id.Value));
    }

    private readonly IItemRepository _itemRepository;
    private readonly ICrafterRepository _crafterRepository;
    private readonly IElementRepository _elementRepository;
    private readonly ICorrelationContextAccessor _correlationContextAccessor;
    private readonly IExceptionToMessageMapper _exceptionToMessageMapper;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICommandHandler<CreateItem> _commandHandler;

    public CreateItemHandlerTests()
    {
        _itemRepository = Substitute.For<IItemRepository>();
        _crafterRepository = Substitute.For<ICrafterRepository>();
        _elementRepository = Substitute.For<IElementRepository>();
        _correlationContextAccessor = Substitute.For<ICorrelationContextAccessor>();
        _exceptionToMessageMapper = Substitute.For<IExceptionToMessageMapper>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        
        _commandHandler = new CreateItemHandler
        (
            _itemRepository,
            _crafterRepository,
            _elementRepository,
            _correlationContextAccessor,
            _exceptionToMessageMapper,
            _eventProcessor
        );
    }
    
    private Item GetItem()
        => Item.Create("Sword", "Deals a lot of damage", 20.30m, DateTimeOffset.Now);
    
    private Crafter GetCrafter()
        => Crafter.Create("Din Boo");
    
    private Element GetElement()
        => Element.Create(Elements.Fire.ToString());
}