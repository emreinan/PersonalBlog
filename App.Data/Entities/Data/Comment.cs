﻿
using App.Data.Entities.Auth;

namespace App.Data.Entities;

public class Comment : Entity
{
    public string Content { get; set; }
    public bool IsApproved { get; set; } = false;
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }

    public virtual User User { get; set; } = default!;
    public virtual BlogPost Post { get; set; } = default!;
}