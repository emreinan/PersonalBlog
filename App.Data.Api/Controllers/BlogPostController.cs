using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.BlogPost;
using App.Shared.Dto.File;
using App.Shared.Services.File;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogPostController(DataDbContext context,IFileService fileService,IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBlogPosts()
    {
        var blogPosts = await context.BlogPosts.Include(b => b.Comments).ToListAsync();

        var blogPostsDto = mapper.Map<List<BlogPostResponseDto>>(blogPosts);
        return Ok(blogPostsDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlogPost(Guid id)
    {
        var blogPost = await context.BlogPosts.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == id);

        if (blogPost == null)
            return NotFound();

        var blogPostDto = mapper.Map<BlogPostResponseDto>(blogPost);

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
            ImageUrl = uploadResult.Value
        };

        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();
        var blogPostResponse = mapper.Map<BlogPostResponseDto>(blogPost);

        return CreatedAtAction(nameof(GetBlogPost), new { id = blogPost.Id }, blogPostResponse);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlogPost(Guid id, [FromForm] BlogPostDto blogPostDto)
    {
        var blogPost = await context.BlogPosts.FindAsync(id);

        if (blogPost == null)
            return NotFound();

        if (blogPostDto.Image != null)
        {
            var result = await fileService.UploadFileAsync(blogPostDto.Image);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            blogPost.ImageUrl = result.Value;
        }

        blogPost.Title = blogPostDto.Title;
        blogPost.Content = blogPostDto.Content;
        blogPost.UpdatedAt = DateTime.UtcNow;

        context.BlogPosts.Update(blogPost);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlogPost(Guid id)
    {
        var blogPost = await context.BlogPosts.FindAsync(id);

        if (blogPost == null)
            return NotFound();

        context.BlogPosts.Remove(blogPost);
        await context.SaveChangesAsync();

        return NoContent();
    }
}

