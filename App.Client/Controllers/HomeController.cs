using App.Shared.Dto.Comment;
using App.Shared.Dto.ContactMessage;
using App.Shared.Models;
using App.Shared.Services.AboutMe;
using App.Shared.Services.BlogPost;
using App.Shared.Services.Comment;
using App.Shared.Services.ContactMessage;
using App.Shared.Services.File;
using App.Shared.Services.PersonalInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace App.Client.Controllers;

public class HomeController(
    IAboutMeService aboutMeservice,
    ICommentService commentService,
    IBlogPostService blogPostService,
    IContactMessageService contactMessageService,
    IPersonalInfoService personalInfoService,
    IFileService fileService
    ) : Controller
{
    public async Task<IActionResult> Index()
    {
        var aboutMe = await aboutMeservice.GetAboutMeAsync();
        var personalInfo = await personalInfoService.GetPersonalInfoAsync();
               
        ViewBag.PersonalInfo = personalInfo;
        ViewBag.AboutMe = aboutMe;

        return View();
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

        var contactMessageDto = new ContactMessageAddDto
        {
            Name = contactMessageViewModel.Name,
            Email = contactMessageViewModel.Email,
            Subject = contactMessageViewModel.Subject,
            Message = contactMessageViewModel.Message
        };
        await contactMessageService.AddContactMessageAsync(contactMessageDto);

        TempData["SuccessMessage"] = "Your message has been submitted successfully!";
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> BlogPost(Guid postId)
    {
        var post = await blogPostService.GetBlogPostAsync(postId);

        var comments = await commentService.GetCommentsForPostAsync(postId);
        var approvedComments = comments.Where(x => x.IsApproved).ToList();

        post.Comments = approvedComments;

        return View(post);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SubmitComment(CommentSendViewModel commentSendViewModel)
    {
        if (!ModelState.IsValid)
            return View(commentSendViewModel);

        var userMail = GetUserMail();
        var userName = GetUserName();
        var userId = GetUserId();

        var commentDto = new CommentDto
        {
            Content = commentSendViewModel.Content,
            PostId = commentSendViewModel.PostId,
            UserId = userId
        };

        await commentService.CreateCommentAsync(commentDto);

        TempData["SuccessMessage"] = "Your comment has been submitted successfully!";
        return RedirectToAction("BlogPost", new { postId = commentDto.PostId });
    }

    [HttpGet("DownloadCv")]
    public async Task<IActionResult> DownloadCv()
    {
        var aboutMe = await aboutMeservice.GetAboutMeAsync();
        if (aboutMe.Cv == null)
            return NotFound("Cv not found.");

        var file = await fileService.GetFileAsync(aboutMe.Cv);

        return File(file, "application/pdf", "Emre-�nan-Eng-Cv.pdf");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
