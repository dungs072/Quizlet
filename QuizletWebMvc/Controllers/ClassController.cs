using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Class;
using QuizletWebMvc.Services.Terminology;
using QuizletWebMvc.ViewModels.Class;
using QuizletWebMvc.ViewModels.Terminology;

namespace QuizletWebMvc.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassService classService;
        public ClassController(IClassService classService)
        {
            this.classService = classService;
        }
        public async Task<IActionResult> YourOwnClass()
        {
            ListClassViewModel listClassViewModel = new ListClassViewModel();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                var classes = await classService.GetClassesByUser(userId);
                listClassViewModel.Classes = classes;
            }
            return View(listClassViewModel);
        }

        public IActionResult CreateYourOwnClass()
        {
            ClassViewModel cla = new ClassViewModel();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                cla.UserId = userId;
            }
            return View(cla);
        }
        public async Task<IActionResult> HandleCreateClass(ClassViewModel classModel)
        {
            ModelState.Remove("LearningModule");
            if (!ModelState.IsValid)
            {
                return View("CreateYourOwnClass", classModel);
            }
            var canCreate = await classService.CreateClass(classModel);
            if (!canCreate)
            {
                TempData["Error"] = "Duplicate class name. Please fix it!!";
                return View("CreateYourOwnClass", classModel);
            }

            TempData["Success"] = "Create a class successfully";
            return RedirectToAction("YourOwnClass");
        }
    }
}
