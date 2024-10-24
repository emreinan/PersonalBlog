namespace App.Shared.Models;

public class UserViewModel

{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? ProfilePhoto { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
