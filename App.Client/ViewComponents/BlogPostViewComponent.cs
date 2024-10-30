using App.Shared.Models;
using App.Shared.Services.BlogPost;
using App.Shared.Services.Comment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace App.Client.ViewComponents;

public class BlogPostViewComponent(IBlogPostService blogPostService,ICommentService commentService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var posts = await blogPostService.GetBlogPostsAsync();

        var postViewModels = new List<BlogPostViewModel>();

        foreach (var post in posts)
        {
            var comments = await commentService.GetCommentsForPostAsync(post.Id);

            var blogPostViewModel = new BlogPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                CreatedAt = post.CreatedAt,
                Comments = comments
            };

            postViewModels.Add(blogPostViewModel);
        }
        return View(posts);
    }
}

