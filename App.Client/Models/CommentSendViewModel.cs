using System.ComponentModel.DataAnnotations;

namespace App.Client.Models;

public class CommentSendViewModel
{
    [Required, MinLength(2), MaxLength(500)]
    public string Content { get; set; }
    [Required]
    public Guid PostId { get; set; }

}

