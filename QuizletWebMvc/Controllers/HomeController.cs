using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Models;
using QuizletWebMvc.Services.Achivement;
using QuizletWebMvc.ViewModels.Achivement;
using QuizletWebMvc.ViewModels.User;
using System.Diagnostics;
using System.Reflection;

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

        public async Task<IActionResult> Index()
        {
            UserAchivement userAchivement = new UserAchivement();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                List<LevelTerms> levelTerms = await achivement.GetLevelTerm(userId);
               
                userAchivement.LevelTerms = levelTerms;
            }
            MarkAttendance mark = new MarkAttendance();
            mark.UserId = userId;
            bool canMark = await achivement.MarkAttendance(mark);
            if (canMark)
            {
                TempData["Success"] = "Hello my friend. Your attendance today is marked!!";
            }
            return View(userAchivement);
        }
        [HttpGet]
        public async Task<IActionResult> LoadMoreAchivement(int pageNumber)
        {
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId)) { }
            UserAchivement userAchivement = new UserAchivement();
            if(pageNumber==1) 
            {
                AchieveStatistics achieveStatistics = await achivement.GetAchieveStatistics(userId);
                userAchivement.AchieveStatistics = achieveStatistics;
                return PartialView("PartialView1", userAchivement);
            }
            if(pageNumber==2)
            {
                List<string> sequenceDates = await achivement.GetSequenceDates(userId);
                userAchivement.SequenceDates = sequenceDates;
                return PartialView("PartialView2",userAchivement);
            }
            if(pageNumber==3)
            {
                List<Badge> badges = await achivement.GetBadges(userId);
                userAchivement.badges = badges;
                return PartialView("PartialView3", userAchivement);
            }
            return Json(false);
            
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