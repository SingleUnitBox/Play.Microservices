namespace Play.User.Core.DTO;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public Dictionary<string, IEnumerable<string>> Claims { get; set; }
    public string State { get; set; }
}