using App.Shared.Dto.BlogPost;
using App.Shared.Models;

namespace App.Shared.Services.BlogPost;

public interface IBlogPostService
{
    Task<List<BlogPostViewModel>> GetBlogPosts();
    Task<BlogPostViewModel> GetBlogPost(Guid postId);
    Task CreateBlogPost(BlogPostDto blogPostDto);
    Task UpdateBlogPost(Guid id, BlogPostUpdateDto blogPostUpdateDto);
    Task DeleteBlogPost(Guid id);
}
