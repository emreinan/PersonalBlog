
namespace App.Shared.Dto.Comment;

public class CommentResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Author { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public string? UserImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
