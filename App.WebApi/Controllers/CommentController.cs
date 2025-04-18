using App.Data.Contexts;
using App.Data.Entities;
using App.Shared.Dto.Comment;
using App.Shared.Services.Mail;
using App.Shared.Util.ExceptionHandling.Types;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(AppDbContext appDbContext,IMapper mapper,IMailService mailService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        var comment = await appDbContext.Comments
            .Include(c => c.Post)  
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment is null)
            return NotFound("Comment not found.");

        var user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == comment.UserId);

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
        var comments = await appDbContext.Comments.ToListAsync() 
            ?? throw new NotFoundException("Comments not found.");

        var commentDtos = await MapCommentsToResponseDtos(comments);
        return Ok(commentDtos);
    }

    [HttpGet("PostComment/{postId}")]
    public async Task<IActionResult> PostComment(Guid postId)
    {
        var comments = await appDbContext.Comments
            .Where(c => c.PostId == postId)
            .ToListAsync();

        var commentDtos = await MapCommentsToResponseDtos(comments);
        return Ok(commentDtos);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CommentDto commentDto)
    {
        var post = await appDbContext.BlogPosts.FindAsync(commentDto.PostId)
            ?? throw new NotFoundException("Comment not found.");

        var comment = mapper.Map<Comment>(commentDto);
        comment.CreatedAt = DateTime.UtcNow;

        await appDbContext.Comments.AddAsync(comment);
        await appDbContext.SaveChangesAsync();

        var adminEmail = "emreinannn@gmail.com"; 
        var subject = "New Comment Submitted";
        var htmlMessage = $"A new comment has been submitted for the post titled '{post.Title}'.<br/>" +
                          $"Comment Content: {comment.Content}";

        await mailService.SendEmailAsync(adminEmail, subject, htmlMessage);

        return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, commentDto);

    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentUpdateDto commentDto)
    {
        var comment = await appDbContext.Comments.FindAsync(id)
            ?? throw new NotFoundException("Comment not found.");

        comment.Content = commentDto.Content;
        comment.UpdatedAt = DateTime.UtcNow;
    
        appDbContext.Comments.Update(comment);
        await appDbContext.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await appDbContext.Comments.FindAsync(id)
            ?? throw new NotFoundException("Comment not found.");

        appDbContext.Comments.Remove(comment);
        await appDbContext.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("Approve/{id}")]
    public async Task<IActionResult> ApproveComment(int id)
    {
        var comment = await appDbContext.Comments.FindAsync(id) 
            ?? throw new NotFoundException("Comment not found.");

        if (comment.IsApproved)
            throw new BadRequestException("Comment is already approved.");

        comment.IsApproved = true;

        appDbContext.Comments.Update(comment);
        await appDbContext.SaveChangesAsync();

        return Ok("Comment approved.");
    }
    private async Task<List<CommentResponse>> MapCommentsToResponseDtos(List<Comment> comments)
    {
        var userIds = comments
            .Select(c => c.UserId)
            .Distinct()
            .ToList();

        var users = await appDbContext.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u);

        return comments.Select(comment =>
        {
            users.TryGetValue(comment.UserId, out var user);

            return new CommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                IsApproved = comment.IsApproved,
                CreatedAt = comment.CreatedAt,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Author = user?.UserName ?? "Unknown User",
                UserImageUrl = user?.ProfilePhotoUrl ?? string.Empty
            };
        }).ToList();
    }
}
