namespace App.Data.Entities;

public class RefreshToken : Entity<Guid>
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? Revoked { get; set; }
    public string? ReasonRevoked { get; set; }

    public virtual User User { get; set; } = default!;
}