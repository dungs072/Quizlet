using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Class;
using QuizletWebMvc.ViewModels.Class;

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
        public async Task<IActionResult> EditYourOwnClass(int classId)
        {
            ClassViewModel cla = await classService.GetClass(classId);
            return View(cla);
        }
        public async Task<IActionResult> HandleEditYourOwnClass(ClassViewModel cla)
        {
            //ModelState.Remove("LearningModule");
            if (!ModelState.IsValid) return View("EditYourOwnClass", cla);
            var canUpdate = await classService.UpdateClass(cla);
            if (!canUpdate)
            {
                TempData["Error"] = "Duplicate class name. Please fix it!!";
                return View("EditYourOwnClass", cla);
            }
            TempData["Success"] = "Update class sucessfully";
            return RedirectToAction("YourOwnClass");
        }
        public async Task<IActionResult> DeleteClass(int classId)
        {
            var canDelete = await classService.DeleteClass(classId);

            if (!canDelete)
            {
                TempData["Error"] = "Delete this class failed because it does not exist";
            }
            else
            {
                TempData["Success"] = "Delete a class sucessfully";
            }

            return RedirectToAction("YourOwnClass");
        }
        public async Task<IActionResult> ShowDetailOwnClassLearningModule(int classId)
        {
            HttpContext.Session.SetString("CurrentClassId",classId.ToString());
            List<ClassLearningModuleViewModel> models = await classService.GetDetailLearningModuleClass(classId);
            ClassViewModel cla = await classService.GetClass(classId);
            ListClassLearningModuleViewModel listModels = new ListClassLearningModuleViewModel();
            listModels.LearningModules = models;
            listModels.Copy(cla);
            listModels.ClassId = classId;
            return View("DetailOwnClass", listModels);

        }
        public async Task<IActionResult> TitleSelection(int classId)
        {
            ListChoiceTitle listChoiceTitle = new ListChoiceTitle();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                listChoiceTitle.TitleChoiceViewModels = await classService.GetTitleDatas(userId);
                listChoiceTitle.ClassId = classId;
            }
            return View(listChoiceTitle);
        }
        public async Task<IActionResult> LearningModuleSelection(int titleId)
        {
            ListLearningModuleViewModel listLearningModule = new ListLearningModuleViewModel();
            listLearningModule.Modules = await classService.GetModuleDatas(titleId);
            listLearningModule.TitleId = titleId;
            return View(listLearningModule);
        }


    }
}
