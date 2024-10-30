using App.Shared.Dto.BlogPost;
using App.Shared.Models;

namespace App.Shared.Services.BlogPost;

public interface IBlogPostService
{
    Task<List<BlogPostViewModel>> GetBlogPostsAsync();
    Task<BlogPostViewModel> GetBlogPostAsync(Guid postId);
    Task CreateBlogPostAsync(BlogPostDto blogPostDto);
    Task UpdateBlogPostAsync(Guid id, BlogPostUpdateDto blogPostUpdateDto);
    Task DeleteBlogPostAsync(Guid id);
}
