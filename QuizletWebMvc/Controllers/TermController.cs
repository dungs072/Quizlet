using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Achivement;
using QuizletWebMvc.Services.Terminology;
using QuizletWebMvc.ViewModels.Achivement;
using QuizletWebMvc.ViewModels.Terminology;
using System.Collections.Generic;
using System.Reflection;

namespace QuizletWebMvc.Controllers
{
    public class TermController : Controller
    {
        private readonly ITerminologyService terminologyService;
        private readonly IAchivement achivement;
        private readonly string apiKey = "AIzaSyDdwQpFpqzK-c4emQlK5Sy6pTDMVnh5qiY";
        private readonly string bucket = "quizlet-c9cab.appspot.com";
        private readonly string gmail = "sa123@gmail.com";
        private readonly string password = "123456";
        public TermController(ITerminologyService terminologyService,IAchivement achivement)
        {
            this.terminologyService = terminologyService;
            this.achivement = achivement;
        }
        public IActionResult Term(int learningModuleId)
        {
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
            LearningModuleViewModel2 learningModule = await terminologyService.GetLearningModuleViewModel(learningModuleId);
            TermViewModel term = new TermViewModel();
            term.LearningModule = learningModule; 
            return View(term);
        }
        [HttpPost]
        public async Task<IActionResult> HandleCreateTerm(TermViewModel term,IFormFile imageFile)
        {
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
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    var cancellation = new CancellationTokenSource();
                    // Initialize Firebase Storage
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                    var authLink = await auth.SignInWithEmailAndPasswordAsync(gmail, password);

                    var firebaseStorage = new FirebaseStorage(bucket, new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken)
                    });

                    // Specify the path in Firebase Storage
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                    string path = $"images/{fileName}";
                    stream.Seek(0, SeekOrigin.Begin);
                    await firebaseStorage.Child(path).PutAsync(stream, cancellation.Token);
                    term.Image = await firebaseStorage.Child(path).GetDownloadUrlAsync();
                }
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
                        TempData["Success"] = "Successfully, You just achieved new badge. " + state.AchivementName;
                    }

                }
            }
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
            LearningModuleViewModel2 learningModule = await terminologyService.GetLearningModuleViewModel(term.LearningModuleId);
            term.LearningModule = learningModule;
            return View(term);
        }
        public async Task<IActionResult> HandleEditTerm(TermViewModel term, IFormFile imageFile)
        {
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
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    var cancellation = new CancellationTokenSource();
                    // Initialize Firebase Storage
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                    var authLink = await auth.SignInWithEmailAndPasswordAsync(gmail, password);

                    var firebaseStorage = new FirebaseStorage(bucket, new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken)
                    });

                    // Specify the path in Firebase Storage
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                    string path = $"images/{fileName}";
                    stream.Seek(0, SeekOrigin.Begin);
                    await firebaseStorage.Child(path).PutAsync(stream, cancellation.Token);
                    if (term.Image != null)
                    {
                        string fileNameDelete = ExtractFileNameFromUrl(term.Image);
                        string deletePath = $"images/{fileNameDelete}";
                        await firebaseStorage.Child(deletePath).DeleteAsync();
                    }
                    term.Image = await firebaseStorage.Child(path).GetDownloadUrlAsync();
                }
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
        static string ExtractFileNameFromUrl(string url)
        {
            // Use Uri to parse the URL
            Uri uri = new Uri(url);

            // Get the filename from the URL using Path.GetFileName
            string fileName = Path.GetFileName(uri.LocalPath);

            return fileName;
        }

        public async Task<IActionResult> PracticeTerm(int learningModuleId, bool isOwned)
        {
            ListObjectivePack listObjective = new ListObjectivePack();
            List<ObjectivePack> objectives =await terminologyService.GetObjectivePacks(learningModuleId);
            listObjective.ObjectivePacks = objectives;
            listObjective.LearningModuleId = learningModuleId;
            listObjective.IsOwned = isOwned;
            return View(listObjective);
        }
        public async Task<IActionResult> TestTerm(int learningModuleId)
        {
            ListObjectivePack listObjective = new ListObjectivePack();
            List<ObjectivePack> objectives = await terminologyService.GetObjectivePacks(learningModuleId);
            listObjective.ObjectivePacks = objectives;
            listObjective.LearningModuleId = learningModuleId;
            return View(listObjective);
        }
        public async Task<IActionResult> PassDataTest(int termId, bool isRightAnswer)
        {
            ResultQuestion question = new ResultQuestion();
            question.TermId = termId;
            question.IsRightAnswer = isRightAnswer;
            var state = await terminologyService.UpdateTermTest(question);
            return Json(new { success = state });
        }
        public IActionResult TermParticipant(int learningModuleId)
        {
            Task<List<TermViewModel>> listTerm = terminologyService.GetTermByLearningModuleId(learningModuleId);
            Task<LearningModuleViewModel2> learningModuleViewModel = terminologyService.GetLearningModuleViewModel(learningModuleId);
            ListTermViewModel listTermViewModel = new ListTermViewModel();
            listTermViewModel.LearningModuleViewModel = learningModuleViewModel.Result;
            listTermViewModel.Terms = listTerm.Result;
            return View(listTermViewModel);
        }
    }
}
