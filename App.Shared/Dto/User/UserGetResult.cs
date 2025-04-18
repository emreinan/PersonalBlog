
namespace App.Shared.Dto.User;

public class UserGetResult
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? ProfilePhoto { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
