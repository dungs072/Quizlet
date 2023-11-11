using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Login;
using QuizletWebMvc.Services.Terminology;
using QuizletWebMvc.ViewModels.Terminology;
using System.Text.Json;
using System.Text;
using QuizletWebMvc.ViewModels.User;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Numerics;
using System.Net.Http;

namespace QuizletWebMvc.Controllers
{
    public class TitleModuleController : Controller
    {
        private readonly ITerminologyService terminologyService;
        public TitleModuleController(ITerminologyService terminologyService)
        {
            this.terminologyService = terminologyService;
        }
        public IActionResult TitleModule()
        {
            if(int.TryParse(HttpContext.Session.GetString("UserId"),out int userId))
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTitleModule(TitleViewModel titleViewModel)
        {
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

            TitleViewModel titleViewModel = await terminologyService.GetTitleViewModel(titleId);
            return View(titleViewModel);
        }
        public IActionResult EditTitleModule2(TitleViewModel titleViewModel)
        {
            return View("EditTitleModule",titleViewModel);
        }
        public async Task<IActionResult> UpdateTitleModule(TitleViewModel titleViewModel)
        {
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
