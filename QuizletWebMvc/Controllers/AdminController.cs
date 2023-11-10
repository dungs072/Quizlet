using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Admin;
using QuizletWebMvc.ViewModels.Admin;

namespace QuizletWebMvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }
        public async Task<IActionResult> LevelTerm()
        {
            List<LevelTerm> levelTerms = await adminService.GetLevelTerm();
            return View(levelTerms);
        }
        [HttpGet]
        public async Task<IActionResult> EditLevelTerm(int levelId)
        {
            LevelTerm levelTerm = await adminService.GetLevelTerm(levelId);
            return View(levelTerm);
        }
        [HttpPost]
        public async Task<IActionResult> EditLevelTerm(LevelTerm level)
        {
            ModelState.Remove("LevelName");
            if (!ModelState.IsValid) { return View(level); }
            var state = await adminService.UpdateLevelTerm(level);
            if (state)
            {
                TempData["Success"] = "Successfully update level term";
            }
            else
            {
                TempData["Error"] = "Server error. Cannot update level term";
            }
            return RedirectToAction("LevelTerm");
        }
    }
}
