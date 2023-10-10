using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Terminology;
using QuizletWebMvc.ViewModels.Terminology;
using System.Collections.Generic;

namespace QuizletWebMvc.Controllers
{
    public class TermController : Controller
    {
        private readonly ITerminologyService terminologyService;
        public TermController(ITerminologyService terminologyService)
        {
            this.terminologyService = terminologyService;
        }
        public IActionResult Term(int learningModuleId)
        {
            Task<List<TermViewModel>> listTerm =  terminologyService.GetTermByLearningModuleId(learningModuleId);
            Task<LearningModuleViewModel> learningModuleViewModel = terminologyService.GetLearningModuleViewModel(learningModuleId);
            ListTermViewModel listTermViewModel = new ListTermViewModel();
            listTermViewModel.LearningModuleViewModel = learningModuleViewModel.Result;
            listTermViewModel.Terms = listTerm.Result;
            return View(listTermViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> CreateTerm(int learningModuleId)
        {
            LearningModuleViewModel learningModule = await terminologyService.GetLearningModuleViewModel(learningModuleId);
            TermViewModel term = new TermViewModel();
            term.LearningModule = learningModule; 
            return View(term);
        }
        [HttpPost]
        public async Task<IActionResult> HandleCreateTerm(TermViewModel term)
        {
            LearningModuleViewModel learningModule = await terminologyService.GetLearningModuleViewModel(term.LearningModuleId);
            term.LearningModule = learningModule;
            ModelState.Remove("LearningModule");
            if (!ModelState.IsValid)
            {
                return View("CreateTerm", term);
            }
            var canCreate = await terminologyService.CreateTerm(term);
            if (!canCreate)
            {
                TempData["Error"] = "Duplicate terminology name in current learning module. Please fix it!!";
                return View("CreateTerm", term);
            }

            TempData["Success"] = "Create a terminology sucessfully";
            return RedirectToAction("Term", new { learningModuleId = term.LearningModuleId });
        }
        public async Task<IActionResult> DeleteTerm(int termId, int learningModuleId)
        {
            var canDelete = await terminologyService.DeleteTerm(termId);

            if (!canDelete)
            {
                TempData["Error"] = "Delete this terminology failed because it does not exist";
            }
            else
            {
                TempData["Success"] = "Delete a terminology sucessfully";
            }

            return RedirectToAction("Term", new { learningModuleId = learningModuleId });
        }
        public async Task<IActionResult> EditTerm(int termId)
        {
            TermViewModel term = await terminologyService.GetTermViewModel(termId);
            LearningModuleViewModel learningModule = await terminologyService.GetLearningModuleViewModel(term.LearningModuleId);
            term.LearningModule = learningModule;
            return View(term);
        }
        public async Task<IActionResult> HandleEditTerm(TermViewModel term)
        {
            ModelState.Remove("LearningModule");
            if (!ModelState.IsValid) return View("EditTerm", term);
            var canUpdate = await terminologyService.UpdateTerm(term);
            if (!canUpdate)
            {
                TempData["Error"] = "Duplicate term name. Please fix it!!";
                return View("EditTerm", term);
            }
            TempData["Success"] = "Update term sucessfully";
            return RedirectToAction("Term", new { learningModuleId = term.LearningModuleId });
        }
        
        public async Task<IActionResult> PracticeTerm(int learningModuleId)
        {
            ListObjectivePack listObjective = new ListObjectivePack();
            List<ObjectivePack> objectives =await terminologyService.GetObjectivePacks(learningModuleId);
            listObjective.ObjectivePacks = objectives;
            return View(listObjective);
        }
    }
}
