using App.Client.Models;

namespace App.Client.Services.BlogPost;

public interface IBlogPostService
{
    Task<List<BlogPostViewModel>> GetBlogPosts();
    Task<BlogPostViewModel> GetBlogPost(Guid postId);
}
