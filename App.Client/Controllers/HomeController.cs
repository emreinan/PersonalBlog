using App.Client.Models;
using App.Client.Services.AboutMe;
using App.Client.Services.BlogPost;
using App.Client.Services.Comment;
using App.Client.Services.ContactMessage;
using App.Client.Services.Education;
using App.Client.Services.Experience;
using App.Client.Services.PersonalInfo;
using App.Client.Services.Project;
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
    IProjectService projectService
    ) : Controller
{
    public async Task<IActionResult> Index()
    {
        var aboutMe = await aboutMeservice.GetAboutMe();
        var posts = await postService.GetBlogPosts();
        var educations = await educationService.GetEducations();
        var experiences = await experienceService.GetExperiences();
        var personalInfo = await personalInfoService.GetPersonalInfo();
        var projects = await projectService.GetProjects();

        var model = new HomeViewModel
        {
            AboutMe = aboutMe,
            BlogPosts = posts,
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

        return View(model);
    }

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

        var model = new BlogPostViewModel
        {
            Comments = comments
        };

        return View(model);
    }
    public Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
    private string GetUserMail()
    {
        if (User.Identity.IsAuthenticated)
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
        return null;
    }
}
