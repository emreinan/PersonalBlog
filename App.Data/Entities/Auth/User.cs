using App.Data.Entities.Data;


namespace App.Data.Entities.Auth;

public class User : Entity<Guid>
{
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Email { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public bool IsActive { get; set; } = false;
    public string? VerificationCode { get; set; }
    public int RoleId { get; set; } 

    public virtual Role Role { get; set; } = default!;
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = default!;

}
