using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Achivement;
using QuizletWebMvc.Services.Terminology;
using QuizletWebMvc.ViewModels.Achivement;
using QuizletWebMvc.ViewModels.Terminology;

namespace QuizletWebMvc.Controllers
{
    public class LearningModuleController : Controller
    {
        private readonly ITerminologyService terminologyService;
        private readonly IAchivement achivement;
        public LearningModuleController(ITerminologyService terminologyService,IAchivement achivement)
        {
            this.terminologyService = terminologyService;
            this.achivement = achivement;
        }
        public IActionResult LearningModule(int titleId, string titleName, string describe)
        {
            Task<List<LearningModuleViewModel2>> modules = terminologyService.GetLearningModuleByTitleId(titleId);
            ListLearningModuleViewModel modulesList = new ListLearningModuleViewModel();
            TitleViewModel titleViewModel = new TitleViewModel() { TitleId = titleId, TitleName = titleName, Describe = describe };
            modulesList.Modules = modules.Result;
            modulesList.TitleViewModel = titleViewModel;
            return View(modulesList);
        }

        public async Task<IActionResult> ReturnToLearningModule(int titleId)
        {
            Task<List<LearningModuleViewModel2>> modules = terminologyService.GetLearningModuleByTitleId(titleId);
            ListLearningModuleViewModel modulesList = new ListLearningModuleViewModel();
            TitleViewModel titleViewModel = await terminologyService.GetTitleViewModel(titleId);
            modulesList.Modules = modules.Result;
            modulesList.TitleViewModel = titleViewModel;
            return View("LearningModule",modulesList);
        }
        [HttpGet]
        public async Task<IActionResult> CreateLearningModule(int titleId)
        {
            LearningModuleViewModel2 learningModuleViewModel = new LearningModuleViewModel2();
            TitleViewModel titleViewModel = await terminologyService.GetTitleViewModel(titleId);
            learningModuleViewModel.TitleId = titleId;
            learningModuleViewModel.TitleView = titleViewModel;
          
            return View(learningModuleViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> HandleCreateLearningModule(LearningModuleViewModel2 learningModuleViewModel)
        {
            TitleViewModel titleViewModel = await terminologyService.GetTitleViewModel(learningModuleViewModel.TitleId);
            learningModuleViewModel.TitleView = titleViewModel;
            ModelState.Remove("TitleView");
            if (!ModelState.IsValid)
            {
                return View("CreateLearningModule",learningModuleViewModel);
            }
            var canCreate = await terminologyService.CreateLearningModule(learningModuleViewModel);
            if (!canCreate)
            {
                TempData["Error"] = "Duplicate learning module name in current title. Please fix it!!";
                return View("CreateLearningModule", learningModuleViewModel);
            }
            TempData["Success"] = "Create learning module sucessfully";

            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                var state = await achivement.AchieveBadge(userId, "modules");
                if (state != null)
                {
                    AchieveBadge ac = new AchieveBadge();
                    ac.AchievementId = state.AchivementId;
                    ac.UserId = userId;
                    var s = await achivement.AddUpdateAchieve(ac);
                    if(s)
                    {
                        TempData["Success"] = "Successfully, You just achieved new badge. " + state.AchivementName;
                    }
                    
                }
            }
            return RedirectToAction("ReturnToLearningModule", new { titleId = learningModuleViewModel.TitleId});
        }
        public async Task<IActionResult> DeleteLearningModule(int learningModuleId,int TitleId)
        {
            var canDelete = await terminologyService.DeleteLearningModule(learningModuleId);

            if(!canDelete) 
            {
                TempData["Error"] = "Delete learning module failed because it has an terminology";
            }
            else
            {
                TempData["Success"] = "Delete learning module sucessfully";
            }
            
            return RedirectToAction("ReturnToLearningModule", new { titleId = TitleId });
        }
        public async Task<IActionResult> EditLearningModule(int learningModuleId)
        {
            LearningModuleViewModel2 learningModuleViewModel = await terminologyService.GetLearningModuleViewModel(learningModuleId);
            TitleViewModel titleViewModel = await terminologyService.GetTitleViewModel(learningModuleViewModel.TitleId);
            learningModuleViewModel.TitleView = titleViewModel;
            return View(learningModuleViewModel);
        }
        public async Task<IActionResult> HandleEditLearningModule(LearningModuleViewModel2 learningModuleView)
        {
            ModelState.Remove("TitleView");
            if (!ModelState.IsValid) return View("EditLearningModule",learningModuleView);
            var canUpdate = await terminologyService.UpdateLearningModule(learningModuleView);
            if (!canUpdate)
            {
                TempData["Error"] = "Duplicate learning module name. Please fix it!!";
                return View("EditLearningModule", learningModuleView);
            }
            TempData["Success"] = "Update learning module sucessfully";
            return RedirectToAction("ReturnToLearningModule",new {titleId = learningModuleView.TitleId});
        }



    }
}
