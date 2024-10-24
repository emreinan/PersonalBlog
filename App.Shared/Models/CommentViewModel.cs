﻿namespace App.Shared.Models;

public class CommentViewModel
{
    public int Id { get; set; }
    public string Content { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Author { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserImage { get; set; }

}
