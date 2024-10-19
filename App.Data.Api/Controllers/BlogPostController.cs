using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.BlogPost;
using App.Shared.Services.Abstract;
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

        var blogPostsDto = mapper.Map<List<BlogPostDto>>(blogPosts);
        return Ok(blogPostsDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlogPost(Guid id)
    {
        var blogPost = await context.BlogPosts.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == id);

        if (blogPost == null)
            return NotFound();

        var blogPostDto = mapper.Map<BlogPostDto>(blogPost);

        return Ok(blogPostDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlogPost([FromForm] BlogPostDto blogPostDto)
    {
        var uploadResult = await fileService.UploadFileAsync(blogPostDto.Image);

        if (!uploadResult.IsSuccess)
            return BadRequest(uploadResult.Errors); 

        var blogPost = mapper.Map<BlogPost>(blogPostDto);
        blogPost.CreatedAt = DateTime.UtcNow;

        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBlogPost), new { id = blogPost.Id }, blogPostDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlogPost(Guid id, [FromForm] BlogPostDto blogPostDto)
    {
        var blogPost = await context.BlogPosts.FindAsync(id);

        if (blogPost == null)
            return NotFound();

        if (blogPostDto.Image != null)
        {
            var imageUrl = await fileService.UploadFileAsync(blogPostDto.Image);

            if (!imageUrl.IsSuccess)
                return BadRequest(imageUrl.Errors);

            blogPost.ImageUrl = imageUrl.Value;
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

