using App.Shared.Models;
using App.Shared.Services.BlogPost;
using App.Shared.Services.Comment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace App.Client.ViewComponents;

public class BlogPostsViewComponent(IBlogPostService blogPostService,ICommentService commentService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string viewName)
    {
        var posts = await blogPostService.GetBlogPostsAsync();

        foreach (var post in posts)
        {
            var comments = await commentService.GetCommentsForPostAsync(post.Id);
            var approvedComments = comments.Where(x => x.IsApproved).ToList();

            post.Comments = approvedComments;
        }

        switch (viewName)
        {
            case "RecentBlog":
                posts = posts.OrderByDescending(x => x.CreatedAt).Take(3).ToList();
                return View("RecentBlog", posts);

            default:
                return View("Main", posts);
        }
    }
}

