using App.Data.Entities.Data;


namespace App.Data.Entities.Auth;

public class User : Entity<Guid>
{
    public string UserName { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;
    public string Email { get; set; } = string.Empty;
    public string? ProfilePhotoUrl { get; set; } 
    public bool IsActive { get; set; } = false;
    public string? VerificationCode { get; set; }
    public int RoleId { get; set; } 

    public virtual Role Role { get; set; } = default!;
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = default!;

}
