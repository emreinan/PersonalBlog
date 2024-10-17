namespace App.Client.Models;

public class HomeViewModel
{
    public AboutMeViewModel AboutMe { get; set; }
    public List<BlogPostViewModel> BlogPosts { get; set; }
    public List<ExperienceViewModel> Experiences { get; set; }
    public List<EducationViewModel> Educations { get; set; }
    public List<ProjectViewModel> Projects { get; set; }
    public PersonalInfoViewModel PersonalInfo { get; set; }
}


