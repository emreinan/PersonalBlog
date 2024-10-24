using App.Shared.Models;

namespace App.Shared.Services.BlogPost;

public interface IBlogPostService
{
    Task<List<BlogPostViewModel>> GetBlogPosts();
    Task<BlogPostViewModel> GetBlogPost(Guid postId);
}
