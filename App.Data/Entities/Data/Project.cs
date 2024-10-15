﻿using App.Data.Entities.Auth;

namespace App.Data.Entities.Data;

public class Project : Entity<int>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}