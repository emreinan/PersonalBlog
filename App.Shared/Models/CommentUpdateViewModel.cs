using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class CommentUpdateViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Content is required."), MinLength(5)]
    public string Content { get; set; }

}
