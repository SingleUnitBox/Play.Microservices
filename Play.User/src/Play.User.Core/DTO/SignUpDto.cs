namespace Play.User.Core.DTO;

public class SignUpDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public Dictionary<string, IEnumerable<string>> Claims { get; set; }
}