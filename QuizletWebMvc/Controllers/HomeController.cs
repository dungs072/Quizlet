using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Models;
using QuizletWebMvc.Services.Achivement;
using QuizletWebMvc.ViewModels.Achivement;
using QuizletWebMvc.ViewModels.User;
using System.Diagnostics;

namespace QuizletWebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAchivement achivement;

        public HomeController(ILogger<HomeController> logger, IAchivement achivement)
        {
            _logger = logger;
            this.achivement = achivement;
        }

        public async  Task<IActionResult> Index()
        {
            UserAchivement userAchivement = new UserAchivement();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                List<LevelTerms> levelTerms = await achivement.GetLevelTerm(userId);
                userAchivement.LevelTerms = levelTerms;
            }
            return View(userAchivement);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Welcome()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}