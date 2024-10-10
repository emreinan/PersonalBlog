using App.Data.Entities.Data;


namespace App.Data.Entities.Auth;

public class User : Entity<Guid>
{
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = default!;
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = default!;
    public virtual ICollection<Experience> Experiences { get; set; } = default!;
    public virtual ICollection<Education> Educations { get; set; } = default!;
    public virtual ICollection<Project> Projects { get; set; } = default!;
    public virtual ICollection<Comment> Comments { get; set; } = default!;
    public virtual ICollection<BlogPost> BlogPosts { get; set; } = default!;
}
