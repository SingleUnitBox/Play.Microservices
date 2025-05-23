﻿using Play.Common;
using Play.User.Core.Types;

namespace Play.User.Core.Entities;

public class User : IEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public Dictionary<string, IEnumerable<string>> Claims { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string State { get; set; }

    public User(string username, string email, string password, string role, Dictionary<string,
        IEnumerable<string>> claims)
    {
        Username = username;
        Email = email;
        Password = password;
        Role = role;
        Claims = claims;
        CreatedAt = DateTimeOffset.Now;
        State = States.Active.ToString();
    }
}