using App.Client.Services.AboutMe;
using Microsoft.AspNetCore.Mvc;

namespace App.Client.Controllers
{
    public class HomeController(IAboutMeService aboutMeservice) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BlogPost()
        {
            return View();
        }

    }
}
