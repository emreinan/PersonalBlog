using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.Comment;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(DataDbContext datDbContext,IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        var comment = await datDbContext.Comments
            .Include(c => c.Post)  
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment is null)
            return NotFound("Comment not found.");

        var user = await datDbContext.AuthDbContext.Users.FirstOrDefaultAsync(u => u.Id == comment.UserId);

        if (user is null)
            return NotFound("User not found this comment.");

        var commentDto = new CommentResponse
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            PostId = comment.PostId,
            UserId = comment.UserId,
            Author = user.UserName
        };

        return Ok(commentDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetComments()
    {
        var comments = await datDbContext.Comments.ToListAsync();

        if (comments.Count == 0 || comments is null)
            return NotFound("Comments not found.");

        var commentDtos = new List<CommentResponse>();

        foreach (var comment in comments)
        {
            var user = await datDbContext.AuthDbContext.Users.FirstOrDefaultAsync(u => u.Id == comment.UserId);

            if (user is null)
                return NotFound("User not found this comment.");

            var commentDto = new CommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                IsApproved = comment.IsApproved,
                CreatedAt = comment.CreatedAt,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Author = user.UserName,
                UserImageUrl = user.ProfilePhotoUrl ?? string.Empty
            };

            commentDtos.Add(commentDto);
        }
        return Ok(commentDtos);
    }

    [HttpGet("PostComment/{postId}")]
    public async Task<IActionResult> PostComment(Guid postId)
    {
        var comments = await datDbContext.Comments
            .Where(c => c.PostId == postId)
            .ToListAsync();

        if (comments.Count == 0 || comments is null)
            return NotFound("Comments not found.");

        var commentDtos = new List<CommentResponse>();

        foreach (var comment in comments) {
            var user = await datDbContext.AuthDbContext.Users.FirstOrDefaultAsync(u => u.Id == comment.UserId);

            if (user is null)
                return NotFound("User not found this comment.");

            var commentDto = new CommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                IsApproved = comment.IsApproved,
                CreatedAt = comment.CreatedAt,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Author = user.UserName,
                UserImageUrl = user.ProfilePhotoUrl ?? string.Empty
            };

            commentDtos.Add(commentDto);
        }
        return Ok(commentDtos);
    }

    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CommentDto commentDto)
    {
        var post = await datDbContext.BlogPosts.FindAsync(commentDto.PostId);
        if (post == null)
            return NotFound("Blog post not found.");

        var comment = mapper.Map<Comment>(commentDto);
        comment.CreatedAt = DateTime.UtcNow;

        await datDbContext.Comments.AddAsync(comment);
        await datDbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id });

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentUpdateDto commentDto)
    {
        var comment = await datDbContext.Comments.FindAsync(id);
        if (comment == null)
            return NotFound("Comment not found.");

        comment.Content = commentDto.Content;
        comment.UpdatedAt = DateTime.UtcNow;
    
        datDbContext.Comments.Update(comment);
        await datDbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await datDbContext.Comments.FindAsync(id);
        if (comment == null)
            return NotFound("Comment not found.");

        datDbContext.Comments.Remove(comment);
        await datDbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("Approve/{id}")]
    public async Task<IActionResult> ApproveComment(int id)
    {
        var comment = await datDbContext.Comments.FindAsync(id);
        if (comment == null)
            return NotFound("Comment not found.");

        comment.IsApproved = true;

        datDbContext.Comments.Update(comment);
        await datDbContext.SaveChangesAsync();

        return Ok("Comment approved.");
    }

}
