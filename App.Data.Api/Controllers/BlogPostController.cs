using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.BlogPost;
using App.Shared.Dto.Comment;
using App.Shared.Services.File;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Security.Claims;

namespace App.Data.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogPostController(DataDbContext dataDbContext, IFileService fileService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBlogPosts()
    {
        var blogPosts = await dataDbContext.BlogPosts.Include(b => b.Comments).ToListAsync();
        if (blogPosts.Count == 0 || blogPosts is null) return NotFound("Blog posts not found.");

        var blogPostsDtos = new List<BlogPostResponse>();

        foreach (var blogPost in blogPosts)
        {
            var user = await dataDbContext.AuthDbContext.Users.FirstOrDefaultAsync(u => u.Id == blogPost.AuthorId);
            if (user == null) return NotFound("BlogPost author not found");

            var blogPostDto = new BlogPostResponse
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                ImageUrl = blogPost.ImageUrl,
                CreatedAt = blogPost.CreatedAt,
                AuthorId = blogPost.AuthorId,
                Author = user.UserName,
                Comments = new List<CommentResponse>()
            };

            foreach (var comment in blogPost.Comments)
            {
                var commentAuthor = await dataDbContext.AuthDbContext.Users.FirstOrDefaultAsync(u => u.Id == comment.UserId);
                if (commentAuthor == null)
                    return NotFound("Comment author not found");

                var commentDto = new CommentResponse
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    PostId = comment.PostId,
                    UserId = comment.UserId,
                    Author = commentAuthor.UserName,
                    IsApproved = comment.IsApproved,
                    CreatedAt = comment.CreatedAt,
                    UserImageUrl = commentAuthor.ProfilePhotoUrl ?? string.Empty
                };
                blogPostDto.Comments.Add(commentDto);
            }
            blogPostsDtos.Add(blogPostDto);
        }
        return Ok(blogPostsDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlogPost(Guid id)
    {
        var blogPost = await dataDbContext.BlogPosts.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == id);
        if (blogPost == null) return NotFound("BlogPost not found");

        var blogUser = await dataDbContext.AuthDbContext.Users.FirstOrDefaultAsync(u => u.Id == blogPost.AuthorId);
        if (blogUser == null) return NotFound("User not found");

        var commentResponses = new List<CommentResponse>();

        foreach (var comment in blogPost.Comments)
        {
            var commentUser = await dataDbContext.AuthDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == comment.UserId);

            commentResponses.Add(new CommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Author = commentUser?.UserName ?? "Unknown User",
                IsApproved = comment.IsApproved,
                CreatedAt = comment.CreatedAt,
                UserImageUrl = commentUser?.ProfilePhotoUrl ?? string.Empty
            });
        }

        var blogPostDto = new BlogPostResponse
        {
            Id = blogPost.Id,
            Title = blogPost.Title,
            Content = blogPost.Content,
            ImageUrl = blogPost.ImageUrl,
            CreatedAt = blogPost.CreatedAt,
            AuthorId = blogPost.AuthorId,
            Author = blogUser.UserName,
            Comments = commentResponses
        };
        return Ok(blogPostDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlogPost([FromForm] BlogPostDto blogPostDto)
    {
        var uploadResult = await fileService.UploadFileAsync(blogPostDto.Image);

        if (!uploadResult.IsSuccess)
            return BadRequest(uploadResult.Errors);

        var blogPost = new BlogPost
        {
            Title = blogPostDto.Title,
            Content = blogPostDto.Content,
            ImageUrl = uploadResult.Value,
            AuthorId = blogPostDto.AuthorId
        };

        dataDbContext.BlogPosts.Add(blogPost);
        await dataDbContext.SaveChangesAsync();
        var blogPostResponse = mapper.Map<BlogPostResponse>(blogPost);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlogPost(Guid id, [FromForm] BlogPostUpdateDto blogPostUpdateDto)
    {
        var blogPost = await dataDbContext.BlogPosts.FindAsync(id);
        if (blogPost == null) return NotFound("BlogPost not found");

        if (blogPostUpdateDto.Image != null)
        {
            var result = await fileService.UploadFileAsync(blogPostUpdateDto.Image);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            blogPost.ImageUrl = result.Value;
        }

        blogPost.Title = blogPostUpdateDto.Title;
        blogPost.Content = blogPostUpdateDto.Content;
        blogPost.UpdatedAt = DateTime.UtcNow;

        dataDbContext.BlogPosts.Update(blogPost);
        await dataDbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlogPost(Guid id)
    {
        var blogPost = await dataDbContext.BlogPosts.FindAsync(id);
        if (blogPost == null) return NotFound("BlogPost not found");

        dataDbContext.BlogPosts.Remove(blogPost);
        try
        {
            await dataDbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        return NoContent();
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}

