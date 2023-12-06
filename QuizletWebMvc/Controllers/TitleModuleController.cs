using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Login;
using QuizletWebMvc.Services.Terminology;
using QuizletWebMvc.ViewModels.Terminology;
using System.Text.Json;
using System.Text;
using QuizletWebMvc.Services.Token;
using System.Security.Claims;

namespace QuizletWebMvc.Controllers
{
    public class TitleModuleController : Controller
    {
        private readonly ITerminologyService terminologyService;
        private readonly ITokenService tokenService;
        public TitleModuleController(ITerminologyService terminologyService, ITokenService tokenService)
        {
            this.terminologyService = terminologyService;
            this.tokenService = tokenService;
        }
        private bool CheckCurrentToken()
        {
            string token = Request.Cookies["AuthToken"];
            if (token == null) { return false; }
            ClaimsPrincipal principal = tokenService.ValidateToken(token);
            return principal != null;
        }
        public IActionResult TitleModule()
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            if (int.TryParse(HttpContext.Session.GetString("UserId"),out int userId))
            {
                Task<List<TitleViewModel>> titles = terminologyService.GetTitlesBaseOnUserId(userId);
                ListTitleViewModel titleViewModel = new ListTitleViewModel();
                titleViewModel.Titles = titles.Result;
                return View(titleViewModel);
            }
            return View();
           
        }
      
        public IActionResult CreateTitleModule()
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTitleModule(TitleViewModel titleViewModel)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            if (!ModelState.IsValid) return View(titleViewModel);
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                titleViewModel.UserId = userId;
                var canCreate = await terminologyService.CreateTitle(titleViewModel);
                if (!canCreate)
                {
                    TempData["Error"] = "Duplicate title name. Please fix it!!";
                    return View(titleViewModel);
                }
            }
            
            TempData["Success"] = "Create title sucessfully";
            return RedirectToAction("TitleModule");
        }
        public async Task<IActionResult> EditTitleModule(int titleId)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }

            TitleViewModel titleViewModel = await terminologyService.GetTitleViewModel(titleId);
            return View(titleViewModel);
        }
        public IActionResult EditTitleModule2(TitleViewModel titleViewModel)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            return View("EditTitleModule",titleViewModel);
        }
        public async Task<IActionResult> UpdateTitleModule(TitleViewModel titleViewModel)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            if (!ModelState.IsValid) return View(titleViewModel);
            var canUpdate = await terminologyService.UpdateTitle(titleViewModel);
            if (!canUpdate)
            {
                TempData["Error"] = "Duplicate title name. Please fix it!!";
                return EditTitleModule2(titleViewModel);
            }
            
            TempData["Success"] = "Update title sucessfully";
            return RedirectToAction("TitleModule");
        }
        
        public async Task<IActionResult> DeleteTitleModule(int titleId)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            var state = await terminologyService.DeleteTitle(titleId);
            if(state)
            {
                TempData["Success"] = "Delete title sucessfully";
            }
            else
            {
                TempData["Error"] = "Your title you want to delete that has learning modules";
            }
            
            return RedirectToAction("TitleModule");
        }


    }
}
