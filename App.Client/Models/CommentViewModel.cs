namespace App.Client.Models;

public class CommentViewModel
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }

    public UserDto User { get; set; }

}
public class UserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
}
