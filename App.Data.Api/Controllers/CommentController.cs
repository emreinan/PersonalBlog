using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.Comment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(DataDbContext _context) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.User)  
            .Include(c => c.Post)  
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
            return NotFound("Comment not found.");

        return Ok(comment);
    }

    [HttpGet("PostComment/{postId}")]
    public async Task<IActionResult> GetCommentsByPost(Guid postId)
    {
        var comments = await _context.Comments
            .Where(c => c.PostId == postId)
            .Include(c => c.User)  
            .ToListAsync();

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CommentDto commentDto)
    {
        var post = await _context.BlogPosts.FindAsync(commentDto.PostId);
        if (post == null)
            return NotFound("Blog post not found.");

        var comment = new Comment
        {
            Content = commentDto.Content,
            UserId = commentDto.UserId,
            PostId = commentDto.PostId,
            IsApproved = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return Ok(comment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto commentDto)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
            return NotFound("Comment not found.");

        comment.Content = commentDto.Content;
        comment.UpdatedAt = DateTime.UtcNow;
    
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
            return NotFound("Comment not found.");

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("Approve/{id}")]
    public async Task<IActionResult> ApproveComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
            return NotFound("Comment not found.");

        comment.IsApproved = true;

        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();

        return Ok("Comment approved.");
    }

}
