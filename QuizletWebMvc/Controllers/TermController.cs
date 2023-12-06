using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Achivement;
using QuizletWebMvc.Services.Firebase;
using QuizletWebMvc.Services.Terminology;
using QuizletWebMvc.Services.Token;
using QuizletWebMvc.ViewModels.Achivement;
using QuizletWebMvc.ViewModels.Terminology;
using System.Security.Claims;

namespace QuizletWebMvc.Controllers
{
    public class TermController : Controller
    {
        private readonly ITerminologyService terminologyService;
        private readonly IAchivement achivement;
        private readonly IFirebaseService firebaseService;
        private readonly ITokenService tokenService;
        public TermController(ITerminologyService terminologyService,IAchivement achivement,
                            IFirebaseService firebaseService,ITokenService tokenService)
        {
            this.terminologyService = terminologyService;
            this.achivement = achivement;
            this.firebaseService = firebaseService;
            this.tokenService = tokenService;
        }
        private bool CheckCurrentToken()
        {
            string token = Request.Cookies["AuthToken"];
            if (token == null) { return false; }
            ClaimsPrincipal principal = tokenService.ValidateToken(token);
            return principal != null;
        }
        public IActionResult Term(int learningModuleId)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            Task<List<TermViewModel>> listTerm =  terminologyService.GetTermByLearningModuleId(learningModuleId);
            Task<LearningModuleViewModel2> learningModuleViewModel = terminologyService.GetLearningModuleViewModel(learningModuleId);
            ListTermViewModel listTermViewModel = new ListTermViewModel();
            listTermViewModel.LearningModuleViewModel = learningModuleViewModel.Result;
            listTermViewModel.Terms = listTerm.Result;
            return View(listTermViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> CreateTerm(int learningModuleId)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            LearningModuleViewModel2 learningModule = await terminologyService.GetLearningModuleViewModel(learningModuleId);
            TermViewModel term = new TermViewModel();
            term.LearningModule = learningModule; 
            return View(term);
        }
        [HttpPost]
        public async Task<IActionResult> HandleCreateTerm(TermViewModel term,IFormFile imageFile)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            LearningModuleViewModel2 learningModule = await terminologyService.GetLearningModuleViewModel(term.LearningModuleId);
            term.LearningModule = learningModule;
            ModelState.Remove("LearningModule");
            ModelState.Remove("imageFile");
            ModelState.Remove("Image");
            if (!ModelState.IsValid)
            {
                return View("CreateTerm", term);
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                term.Image = await firebaseService.StoreImage(imageFile, "images");
            }
            var canCreate = await terminologyService.CreateTerm(term);
            if (!canCreate)
            {
                TempData["Error"] = "Duplicate terminology name in current learning module. Please fix it!!";
                return View("CreateTerm", term);
            }
            TempData["Success"] = "Create a terminology sucessfully";
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                var state = await achivement.AchieveBadge(userId, "terms");
                if (state != null)
                {
                    AchieveBadge ac = new AchieveBadge();
                    ac.AchievementId = state.AchivementId;
                    ac.UserId = userId;
                    var s = await achivement.AddUpdateAchieve(ac);
                    if (s)
                    {
                        TempData["Success"] = "Successfully, You just achieved new badge. " + state.AchivementName.Split(',')[0];
                    }

                }
            }
            return RedirectToAction("Term", new { learningModuleId = term.LearningModuleId });
        }
        public async Task<IActionResult> DeleteTerm(int termId, int learningModuleId)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
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
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            TermViewModel term = await terminologyService.GetTermViewModel(termId);
            LearningModuleViewModel2 learningModule = await terminologyService.GetLearningModuleViewModel(term.LearningModuleId);
            term.LearningModule = learningModule;
            return View(term);
        }
        public async Task<IActionResult> HandleEditTerm(TermViewModel term, IFormFile imageFile)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            ModelState.Remove("LearningModule");
            ModelState.Remove("Image");
            ModelState.Remove("imageFile");
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Error. Please fix it!!";
                return View("EditTerm", term);
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                if(term.Image!=null)
                {
                   await firebaseService.DeleteImage(term.Image, "images");
                }
                term.Image = await firebaseService.StoreImage(imageFile, "images");
            }
            var canUpdate = await terminologyService.UpdateTerm(term);
            if (!canUpdate)
            {
                TempData["Error"] = "Duplicate term name. Please fix it!!";
                return View("EditTerm", term);
            }
            TempData["Success"] = "Edit terminology successfully";
            return RedirectToAction("Term", new { learningModuleId = term.LearningModuleId });
        }

        public async Task<IActionResult> PracticeTerm(int learningModuleId, bool isOwned)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            ListObjectivePack listObjective = new ListObjectivePack();
            List<ObjectivePack> objectives =await terminologyService.GetObjectivePacks(learningModuleId);
            listObjective.ObjectivePacks = objectives;
            listObjective.LearningModuleId = learningModuleId;
            listObjective.IsOwned = isOwned;
            return View(listObjective);
        }
        public async Task<IActionResult> TestTerm(int learningModuleId)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            ListObjectivePack listObjective = new ListObjectivePack();
            List<ObjectivePack> objectives = await terminologyService.GetObjectivePacks(learningModuleId);
            listObjective.ObjectivePacks = objectives;
            listObjective.LearningModuleId = learningModuleId;
            return View(listObjective);
        }
        public async Task<IActionResult> PassDataTest(int termId, bool isRightAnswer)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            ResultQuestion question = new ResultQuestion();
            question.TermId = termId;
            question.IsRightAnswer = isRightAnswer;
            var state = await terminologyService.UpdateTermTest(question);
            return Json(new { success = state });
        }
        public IActionResult TermParticipant(int learningModuleId)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            Task<List<TermViewModel>> listTerm = terminologyService.GetTermByLearningModuleId(learningModuleId);
            Task<LearningModuleViewModel2> learningModuleViewModel = terminologyService.GetLearningModuleViewModel(learningModuleId);
            ListTermViewModel listTermViewModel = new ListTermViewModel();
            listTermViewModel.LearningModuleViewModel = learningModuleViewModel.Result;
            listTermViewModel.Terms = listTerm.Result;
            return View(listTermViewModel);
        }

        public IActionResult Search(int moduleId, string searchTerm)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            Task<List<TermViewModel>> listTerm = terminologyService.GetTermByLearningModuleId(moduleId);
            Task<LearningModuleViewModel2> learningModuleViewModel = terminologyService.GetLearningModuleViewModel(moduleId);
            ListTermViewModel listTermViewModel = new ListTermViewModel();
            listTermViewModel.LearningModuleViewModel = learningModuleViewModel.Result;
            listTermViewModel.Terms = listTerm.Result;
            if (searchTerm!=null&&searchTerm!="")
            {
                List<TermViewModel> searchList = listTerm.Result.Where(a => a.TermName.Contains(searchTerm) || a.Explaination.Contains(searchTerm)).ToList();
                listTermViewModel.Terms = searchList;
            }          
            return View("Term", listTermViewModel);
        }
    }
}
