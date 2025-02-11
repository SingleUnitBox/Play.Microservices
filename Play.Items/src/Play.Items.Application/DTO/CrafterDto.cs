namespace Play.Items.Application.DTO;

public class CrafterDto
{
    public Guid CrafterId { get; set; }
    public string CrafterName { get; set; }
    public IEnumerable<string> Skills { get; set; }
    public IEnumerable<ItemDto> Items { get; set; }
}