using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Dto.Comment;

public class CommentResponse
{
    public int Id { get; set; }
    public string Content { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Author { get; set; }
    public DateTime CreatedAt { get; set; }
}
