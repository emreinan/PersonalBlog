using App.Shared.Dto.BlogPost;
using App.Shared.Dto.Comment;
using App.Shared.Models;
using App.Shared.Services.BlogPost;
using App.Shared.Services.Comment;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;

namespace App.Admin.Controllers;

[Route("BlogPost")]
public class BlogPostController(IBlogPostService blogPostService, ICommentService commentService,IMapper mapper) : Controller
{
    [HttpGet("BlogPosts")]
    public async Task<IActionResult> BlogPosts()
    {
        var blogPosts = await blogPostService.GetBlogPosts();
        var blogpostViewModels = mapper.Map<List<BlogPostViewModel>>(blogPosts);
        return View(blogpostViewModels);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BlogPost(Guid id)
    {
        var blogPost = await blogPostService.GetBlogPost(id);
        var model = mapper.Map<BlogPostViewModel>(blogPost);
        return View(model);
    }

    [HttpGet("CreateBlogPost")]
    public IActionResult CreateBlogPost()
    {
        return View();
    }

    [HttpPost("CreateBlogPost")]
    public async Task<IActionResult> CreateBlogPost(BlogPostCreatedViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = GetUserId();
        if (userId == Guid.Empty) 
            return RedirectToAction("Login", "Auth");

        var blogPostDto = new BlogPostDto { Title = model.Title, Content = model.Content, Image = model.Image , AuthorId = userId};
        await blogPostService.CreateBlogPost(blogPostDto);

        ViewBag.Success = "Blog post created successfully.";
        return RedirectToAction("BlogPosts");
    }

    [HttpGet("EditBlogPost/{id}")]
    public async Task<IActionResult> EditBlogPost(Guid id)
    {
        var blogPost = await blogPostService.GetBlogPost(id);
        var model = new BlogPostEditViewModel { Id = blogPost.Id, Title = blogPost.Title, Content = blogPost.Content };
        return View(model);
    }

    [HttpPost("EditBlogPost/{id}")]
    public async Task<IActionResult> EditBlogPost(Guid id, BlogPostEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var blogPostDto = new BlogPostUpdateDto { Title = model.Title, Content = model.Content, Image = model.Image };
        await blogPostService.UpdateBlogPost(id, blogPostDto);

        return RedirectToAction("BlogPosts");
    }

    [HttpGet("DeleteBlogPost/{id}")]
    public async Task<IActionResult> DeleteBlogPost(Guid id)
    {
        var blogPost = await blogPostService.GetBlogPost(id);

        await blogPostService.DeleteBlogPost(id);
        return RedirectToAction(nameof(BlogPosts));
    }

    [HttpGet("CreateComment/{id}")]
    public IActionResult CreateComment(Guid Id)
    {
        return View();
    }

    [HttpPost("CreateComment/{id}")]
    public async Task<IActionResult> CreateComment(Guid Id, CommentCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = GetUserId();
        if (userId == Guid.Empty)
            return RedirectToAction("Login", "Auth");

        var commentDto = new CommentDto { Content = model.Content, PostId = Id, UserId = userId };
        await commentService.CreateComment(commentDto);

        ViewBag.Success = "Comment created successfully.";
        return RedirectToAction(nameof(BlogPosts));
    }

    private Guid GetUserId()
    {
        if (!User.Identity.IsAuthenticated)
            return Guid.Empty;
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
