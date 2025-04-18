using App.Data.Contexts;
using App.Data.Entities;
using App.Shared.Dto.BlogPost;
using App.Shared.Dto.Comment;
using App.Shared.Services.File;
using App.Shared.Util.ExceptionHandling.Types;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogPostController(AppDbContext appDbContext, IFileService fileService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBlogPosts()
    {
        var blogPosts = await appDbContext.BlogPosts.Include(b => b.Comments).ToListAsync() 
            ?? throw new NotFoundException("Blog posts not found.");

        // Bütün userId'leri topla (author + comment authors)
        var userIds = blogPosts
            .Select(b => b.AuthorId)
            .Union(blogPosts.SelectMany(b => b.Comments.Select(c => c.UserId)))
            .Distinct()
            .ToList();

        // Hepsini tek seferde DB'den al
        var users = await appDbContext.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u);

        var blogPostsDtos = blogPosts.Select(blogPost =>
        {
            users.TryGetValue(blogPost.AuthorId, out var authorUser);

            var blogPostDto = new BlogPostResponse
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                ImageUrl = blogPost.ImageUrl,
                CreatedAt = blogPost.CreatedAt,
                AuthorId = blogPost.AuthorId,
                Author = authorUser?.UserName ?? "Unknown Author",
                Comments = blogPost.Comments.Select(comment =>
                {
                    users.TryGetValue(comment.UserId, out var commentUser);

                    return new CommentResponse
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        PostId = comment.PostId,
                        UserId = comment.UserId,
                        Author = commentUser?.UserName ?? "Anonymous",
                        IsApproved = comment.IsApproved,
                        CreatedAt = comment.CreatedAt,
                        UserImageUrl = commentUser?.ProfilePhotoUrl ?? string.Empty
                    };
                }).ToList()
            };

            return blogPostDto;
        }).ToList();

        return Ok(blogPostsDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlogPost(Guid id)
    {
        var blogPost = await appDbContext.BlogPosts.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == id) 
            ?? throw new NotFoundException("BlogPost not found.");

        // Author + yorumcuların ID'lerini topla
        var userIds = blogPost.Comments
            .Select(c => c.UserId)
            .Append(blogPost.AuthorId)
            .Distinct()
            .ToList();

        // Kullanıcıları tek sorguda çek
        var users = await appDbContext.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u);

        // Blog yazarını bul (null olabilir fallback veriyoruz)
        users.TryGetValue(blogPost.AuthorId, out var blogUser);

        var commentResponses = blogPost.Comments.Select(comment =>
        {
            users.TryGetValue(comment.UserId, out var commentUser);

            return new CommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Author = commentUser?.UserName ?? "Unknown User",
                IsApproved = comment.IsApproved,
                CreatedAt = comment.CreatedAt,
                UserImageUrl = commentUser?.ProfilePhotoUrl ?? string.Empty
            };
        }).ToList();

        var blogPostDto = new BlogPostResponse
        {
            Id = blogPost.Id,
            Title = blogPost.Title,
            Content = blogPost.Content,
            ImageUrl = blogPost.ImageUrl,
            CreatedAt = blogPost.CreatedAt,
            AuthorId = blogPost.AuthorId,
            Author = blogUser?.UserName ?? "Unknown Author",
            Comments = commentResponses
        };

        return Ok(blogPostDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateBlogPost([FromForm] BlogPostDto blogPostDto)
    {
        var uploadResult = await fileService.UploadFileAsync(blogPostDto.Image);

        var blogPost = new BlogPost
        {
            Title = blogPostDto.Title,
            Content = blogPostDto.Content,
            ImageUrl = uploadResult,
            AuthorId = blogPostDto.AuthorId
        };

        appDbContext.BlogPosts.Add(blogPost);
        await appDbContext.SaveChangesAsync();
        var blogPostResponse = mapper.Map<BlogPostResponse>(blogPost);

        return Ok(blogPostResponse);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlogPost(Guid id, [FromForm] BlogPostUpdateDto blogPostUpdateDto)
    {
        var blogPost = await appDbContext.BlogPosts.FindAsync(id);
        if (blogPost is null) return NotFound("BlogPost not found");

        if (blogPostUpdateDto.Image is not null)
        {
            var result = await fileService.UploadFileAsync(blogPostUpdateDto.Image);

            blogPost.ImageUrl = result;
        }

        blogPost.Title = blogPostUpdateDto.Title;
        blogPost.Content = blogPostUpdateDto.Content;
        blogPost.UpdatedAt = DateTime.UtcNow;

        appDbContext.BlogPosts.Update(blogPost);
        await appDbContext.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlogPost(Guid id)
    {
        var blogPost = await appDbContext.BlogPosts.FindAsync(id);
        if (blogPost is null) return NotFound("BlogPost not found");

        appDbContext.BlogPosts.Remove(blogPost);
            await appDbContext.SaveChangesAsync();

        return NoContent();
    }
}

