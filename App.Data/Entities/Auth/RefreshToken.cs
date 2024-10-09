﻿namespace App.Data.Entities.Auth;

public class RefreshToken : Entity
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? Revoked { get; set; }

    public virtual User User { get; set; } = default!;
}