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
    IFileService fileService
    ) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SubmitContactMessage(ContactMessageViewModel contactMessageViewModel)
    {
        if (User.Identity.IsAuthenticated)
        {
            contactMessageViewModel.Email = GetUserMail();
            contactMessageViewModel.Name = GetUserName();

            //Manual giriþleri siliyoruz. Yoksa model state geçersiz oluyor.
            ModelState.Remove("Email");
            ModelState.Remove("Name");
        }
        if (!ModelState.IsValid)
            return View(contactMessageViewModel);

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

        TempData["SuccessMessage"] = "Your comment has been submitted successfully! Please wait for admin approval.";
        return RedirectToAction("BlogPost", new { postId = commentDto.PostId });
    }

    [HttpGet("DownloadCv")]
    public async Task<IActionResult> DownloadCv()
    {
        var aboutMe = await aboutMeservice.GetAboutMeAsync();
        if (aboutMe.Cv == null)
            return NotFound("Cv not found.");

        var file = await fileService.GetDownloadFileAsync(aboutMe.Cv);

        return File(file, "application/pdf", "Emre-Ýnan-Eng-Cv.pdf");
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
}
