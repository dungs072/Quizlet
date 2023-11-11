using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Admin;
using QuizletWebMvc.Services.Class;
using QuizletWebMvc.ViewModels.Admin;

namespace QuizletWebMvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly string apiKey = "AIzaSyDdwQpFpqzK-c4emQlK5Sy6pTDMVnh5qiY";
        private readonly string bucket = "quizlet-c9cab.appspot.com";
        private readonly string gmail = "sa123@gmail.com";
        private readonly string password = "123456";

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
        public async Task<IActionResult> Badge()
        {
            List<Badge> totalBadge = await adminService.GetBadges();
            totalBadge = totalBadge.OrderBy(a=>a.Condition).ToList();
            List<Badge> moduleBadge = new List<Badge>();
            List<Badge> termBadge = new List<Badge>();
            List<Badge> participantBadge = new List<Badge>();
            
            foreach(var badge in totalBadge)
            {
                string[] temp = badge.AchivementName.Split(',');
                badge.AchivementName = temp[0];
                badge.TypeBadge = temp[1];
                if (temp[1].Contains("modules"))
                {
                    moduleBadge.Add(badge);
                }
                else if (temp[1].Contains("terms"))
                {
                    termBadge.Add(badge);
                }
                else if (temp[1].Contains("participants"))
                {
                    participantBadge.Add(badge);
                }
            }

            ListBadges badges = new ListBadges();
            badges.ModuleBadges = moduleBadge;
            badges.TermBadges = termBadge;
            badges.ParticipantBadges = participantBadge;
            return View(badges);
        }

        [HttpGet]
        public async Task<IActionResult> EditBadge(int achivementId,string typeBadge)
        {
            Badge badge = await adminService.GetBadge(achivementId);
            string[] temp = badge.AchivementName.Split(',');
            badge.AchivementName = temp[0];
            badge.TypeBadge = typeBadge;
            return View(badge);
        }
        [HttpPost]
        public async Task<IActionResult> EditBadge(Badge badge, IFormFile imageFile)
        {
            ModelState.Remove("Image");
            ModelState.Remove("imageFile");
            if (!ModelState.IsValid) { return View(badge); }
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
                    string path = $"admin/{fileName}";
                    stream.Seek(0, SeekOrigin.Begin);
                    await firebaseStorage.Child(path).PutAsync(stream, cancellation.Token);
                    if (badge.Image != null)
                    {
                        string fileNameDelete = ExtractFileNameFromUrl(badge.Image);
                        string deletePath = $"admin/{fileNameDelete}";
                        await firebaseStorage.Child(deletePath).DeleteAsync();
                    }
                    badge.Image = await firebaseStorage.Child(path).GetDownloadUrlAsync();
                }
            }
            badge.AchivementName = badge.AchivementName + "," + badge.TypeBadge;
            var state = await adminService.UpdateBadge(badge);
            if (state)
            {
                TempData["Success"] = "Successfully update badge";
            }
            else
            {
                TempData["Error"] = "Server error. Cannot update badge";
            }
            return RedirectToAction("Badge");
        }

        static string ExtractFileNameFromUrl(string url)
        {
            // Use Uri to parse the URL
            Uri uri = new Uri(url);

            // Get the filename from the URL using Path.GetFileName
            string fileName = Path.GetFileName(uri.LocalPath);

            return fileName;
        }

        [HttpGet]
        public IActionResult AddBadge(string typeBadge)
        {
            Badge badge = new Badge();
            badge.TypeBadge = typeBadge;
            return View(badge);
        }
        [HttpPost]
        public async Task<IActionResult> AddBadge(Badge badge, IFormFile imageFile)
        {
            ModelState.Remove("Image");
            ModelState.Remove("imageFile");
            if (!ModelState.IsValid) { return View(badge); }
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
                    string path = $"admin/{fileName}";
                    stream.Seek(0, SeekOrigin.Begin);
                    await firebaseStorage.Child(path).PutAsync(stream, cancellation.Token);
                    badge.Image = await firebaseStorage.Child(path).GetDownloadUrlAsync();
                }
            }
            badge.AchivementName = badge.AchivementName + "," + badge.TypeBadge;
            var state = await adminService.CreateBadge(badge);
            if (state)
            {
                TempData["Success"] = "Successfully add new badge";
            }
            else
            {
                TempData["Error"] = "Server error. Cannot add new badge";
            }
            return RedirectToAction("Badge");
        }

        public async Task<IActionResult> DeleteBadge(int achievementId)
        {
            var canDelete = await adminService.DeleteBadge(achievementId);

            if (!canDelete)
            {
                TempData["Error"] = "Delete this badge failed because it does not exist";
            }
            else
            {
                TempData["Success"] = "Delete a badge sucessfully";
            }

            return RedirectToAction("Badge");
        }
    }
   
}
