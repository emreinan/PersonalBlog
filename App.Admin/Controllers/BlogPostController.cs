﻿using App.Shared.Dto.BlogPost;
using App.Shared.Dto.Comment;
using App.Shared.Models;
using App.Shared.Services.BlogPost;
using App.Shared.Services.Comment;
using App.Shared.Services.File;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;

namespace App.Admin.Controllers;

[Route("BlogPost")]
public class BlogPostController(IBlogPostService blogPostService, ICommentService commentService,IMapper mapper,IFileService fileService) : Controller
{
    [HttpGet("BlogPosts")]
    public async Task<IActionResult> BlogPosts()
    {
        var blogPosts = await blogPostService.GetBlogPostsAsync();
        var blogpostViewModels = mapper.Map<List<BlogPostViewModel>>(blogPosts);
        return View(blogpostViewModels);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BlogPost(Guid id)
    {
        var blogPost = await blogPostService.GetBlogPostAsync(id);
        var model = mapper.Map<BlogPostViewModel>(blogPost);
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("CreateBlogPost")]
    public IActionResult CreateBlogPost()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("CreateBlogPost")]
    public async Task<IActionResult> CreateBlogPost(BlogPostCreatedViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = GetUserId();
        if (userId == Guid.Empty) 
            return RedirectToAction("Login", "Auth");

        var blogPostDto = new BlogPostDto { Title = model.Title, Content = model.Content, Image = model.Image , AuthorId = userId};
        await blogPostService.CreateBlogPostAsync(blogPostDto);

        TempData["SuccessMessage"] = "Blog post created successfully.";
        return RedirectToAction("BlogPosts");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("EditBlogPost/{id}")]
    public async Task<IActionResult> EditBlogPost(Guid id)
    {
        var blogPost = await blogPostService.GetBlogPostAsync(id);
        var model = new BlogPostEditViewModel { Id = blogPost.Id, Title = blogPost.Title, Content = blogPost.Content };
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("EditBlogPost/{id}")]
    public async Task<IActionResult> EditBlogPost(Guid id, BlogPostEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var blogPostDto = new BlogPostUpdateDto { Title = model.Title, Content = model.Content, Image = model.Image };
        await blogPostService.UpdateBlogPostAsync(id, blogPostDto);

        TempData["SuccessMessage"] = "Blog post updated successfully.";
        return RedirectToAction("BlogPosts");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("DeleteBlogPost/{id}")]
    public async Task<IActionResult> DeleteBlogPost(Guid id)
    {
        var blogPost = await blogPostService.GetBlogPostAsync(id);

        await blogPostService.DeleteBlogPostAsync(id);

        TempData["SuccessMessage"] = "Blog post deleted successfully.";
        return RedirectToAction(nameof(BlogPosts));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("CreateCommentAsync/{id}")]
    public IActionResult CreateComment(Guid Id)
    {
        return View();
    }

    [Authorize]
    [HttpPost("CreateCommentAsync/{id}")]
    public async Task<IActionResult> CreateComment(Guid Id, CommentCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = GetUserId();
        if (userId == Guid.Empty)
            return RedirectToAction("Login", "Auth");

        var commentDto = new CommentDto { Content = model.Content, PostId = Id, UserId = userId };
        await commentService.CreateCommentAsync(commentDto);

        TempData["SuccessMessage"] = "Comment created successfully.";
        return RedirectToAction(nameof(BlogPosts));
    }

    [HttpGet("GetImage")]
    public async Task<IActionResult> GetImage(string fileUrl)
    {
        try
        {
            var file = await fileService.GetFileAsync(fileUrl);
            var contentType = GetContentType(fileUrl);
            return File(file, contentType);
        }
        catch (HttpRequestException)
        {
            return NotFound("File not found.");
        }
    }
    private static string GetContentType(string fileUrl)
    {
        var types = new Dictionary<string, string>
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            {".txt", "text/plain"},
            {".pdf", "application/pdf"}
        };

        var ext = Path.GetExtension(fileUrl).ToLowerInvariant();
        return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
    }

    private Guid GetUserId()
    {
        if (!User.Identity.IsAuthenticated)
            return Guid.Empty;
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
