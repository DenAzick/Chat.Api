﻿using Identity.Core.Entities;

namespace Identity.Core.Models;

public class UserModel
{
    public UserModel(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Username = user.Username;
    }
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string Username { get; set; }
}
