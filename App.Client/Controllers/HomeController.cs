using App.Shared.Models;
using App.Shared.Services.AboutMe;
using App.Shared.Services.BlogPost;
using App.Shared.Services.Comment;
using App.Shared.Services.ContactMessage;
using App.Shared.Services.Education;
using App.Shared.Services.Experience;
using App.Shared.Services.PersonalInfo;
using App.Shared.Services.Project;
using App.Shared.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Client.Controllers;

public class HomeController(
    IAboutMeService aboutMeservice,
    IBlogPostService postService,
    ICommentService commentService,
    IContactMessageService contactMessageService,
    IEducationService educationService,
    IExperienceService experienceService,
    IPersonalInfoService personalInfoService,
    IProjectService projectService,
    IUserService userService
    ) : Controller
{
    public async Task<IActionResult> Index()
    {
        var aboutMe = await aboutMeservice.GetAboutMe();
        var posts = await postService.GetBlogPosts();
        var educations = await educationService.GetEducations();
        var experiences = await experienceService.GetExperiences();
        var personalInfo = await personalInfoService.GetPersonalInfoAsync();
        var projects = await projectService.GetProjects();

        var postViewModels = new List<BlogPostViewModel>();

        foreach (var post in posts)
        {
            var comments = await commentService.GetCommentsForPost(post.Id);

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

        var model = new HomeViewModel
        {
            AboutMe = aboutMe,
            BlogPosts = postViewModels,
            Educations = educations,
            Experiences = experiences,
            PersonalInfo = personalInfo,
            Projects = projects,
            PersonalInfoAboutMe = new PersonalInfoAboutMeViewModel
            {
                AboutMe = aboutMe,
                PersonalInfo = personalInfo
            },
            ContactMessage = new ContactMessageViewModel()

        };
        ViewBag.PersonalInfo = personalInfo;
        ViewBag.AboutMe = aboutMe;

        return View(model);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SubmitContactMessage(ContactMessageViewModel contactMessageViewModel)
    {
        if (!ModelState.IsValid)
            return View(contactMessageViewModel);

        var userMail = GetUserMail();
        if (!string.IsNullOrEmpty(userMail))
            contactMessageViewModel.Email = userMail;

        var result = await contactMessageService.AddContactMessage(contactMessageViewModel);

        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while submitting the contact message. Please try again.");
            return View(contactMessageViewModel);
        }
        TempData["SuccessMessage"] = "Your message has been submitted successfully!";
        return RedirectToAction("Index", "Home");

    }

    public async Task<IActionResult> BlogPost(Guid postId)
    {
        var comments = await commentService.GetCommentsForPost(postId);
        var approvedComments = comments.Where(c => c.IsApproved).ToList();

        var blogPost = await postService.GetBlogPost(postId);

        var recentBlogs = await postService.GetBlogPosts();

        var commentViewModels = new List<CommentViewModel>();

        foreach (var comment in approvedComments)
        {
            var commmentUser = await userService.GetUserAsync(comment.UserId);

            if (commmentUser?.Value != null)
            {

                commentViewModels.Add(new CommentViewModel
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    PostId = comment.PostId,
                    UserId = comment.UserId,
                    Author = commmentUser.Value.UserName,
                    CreatedAt = comment.CreatedAt,
                    IsApproved = comment.IsApproved,
                    UserImage = commmentUser.Value.ProfilePhoto!.ToString()
                });
            }
        }
        var model = new BlogPostViewModel
        {
            Id = postId,
            Title = blogPost.Title,
            Content = blogPost.Content,
            ImageUrl = blogPost.ImageUrl,
            CreatedAt = blogPost.CreatedAt,
            Comments = commentViewModels,
            RecentBlogs = recentBlogs.Select(b => new BlogPostViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                ImageUrl = b.ImageUrl,
                CreatedAt = b.CreatedAt
            }).ToList()

        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SubmitComment(CommentSendViewModel commentSendViewModel)
    {
        if (!ModelState.IsValid)
            return View(commentSendViewModel);

        var userMail = GetUserMail();
        var userName = GetUserName();

        var commentViewModel = new CommentViewModel
        {
            Content = commentSendViewModel.Content,
            PostId = commentSendViewModel.PostId,
            UserId = GetUserId(),
            Author = userName
        };
        var result = await commentService.CreateComment(commentViewModel);

        if (result is null)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while submitting the comment. Please try again.");
            return View(commentSendViewModel);
        }

        TempData["SuccessMessage"] = "Your comment has been submitted successfully!";
        return RedirectToAction("BlogPost", new { postId = commentViewModel.PostId });

    }

    public Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
    private string GetUserMail()
    {
        return User.FindFirst(ClaimTypes.Email).Value;
    }
    private string GetUserName()
    {
        return User.FindFirst(ClaimTypes.Name).Value;
    }
}
