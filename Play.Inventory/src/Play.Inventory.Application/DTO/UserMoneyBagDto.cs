﻿namespace Play.Inventory.Application.DTO;

public class UserMoneyBagDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public decimal Gold { get; set; }
}