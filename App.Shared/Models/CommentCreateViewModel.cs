using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class CommentCreateViewModel
{
    [Required, MinLength(5)]
    public string Content { get; set; }
}
