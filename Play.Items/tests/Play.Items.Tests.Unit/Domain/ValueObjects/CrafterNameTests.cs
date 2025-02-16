using Play.Items.Domain.Exceptions;
using Play.Items.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace Play.Items.Tests.Unit.Domain.ValueObjects;

public class CrafterNameTests
{
    [Fact]
    public void create_crafter_name_with_empty_string_input_should_throw_invalid_crafter_name_exception()
    {
        var exception = Record.Exception(() => new CrafterName(string.Empty));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidCrafterNameException>();
    }

    [Fact]
    public void create_crafter_name_with_valid_input_should_succeed()
    {
        var crafterName = new CrafterName("Din Foos");
        
        crafterName.ShouldNotBeNull();
        crafterName.Value.ShouldBe("Din Foos");
    }
}