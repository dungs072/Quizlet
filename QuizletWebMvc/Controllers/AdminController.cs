using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Admin;
using QuizletWebMvc.Services.Class;
using QuizletWebMvc.Services.Firebase;
using QuizletWebMvc.ViewModels.Admin;

namespace QuizletWebMvc.Controllers
{
    public class AdminController : Controller
    {

        private readonly IAdminService adminService;
        private readonly IFirebaseService firebaseService;
        public AdminController(IAdminService adminService, IFirebaseService firebaseService)
        {
            this.adminService = adminService;
            this.firebaseService = firebaseService;
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
                if(badge.Image!=null)
                {
                    await firebaseService.DeleteImage(badge.Image, "admin");
                }
                badge.Image = await firebaseService.StoreImage(imageFile, "admin");
                
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
                    badge.Image = await firebaseService.StoreImage(imageFile, "admin");
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

        public async Task<IActionResult> UserManager()
        {
            List<UserManagerViewModel> users = await adminService.GetUserManagers();
            return View(users);
        }

        public async Task<IActionResult> UpdateStateUserManager(int userId,bool state)
        {
            UserState userState = new UserState();
            userState.UserId = userId;
            userState.State = state;
            var canUpdate = await adminService.UpdateUserState(userState);
            if (!canUpdate)
            {
                TempData["Error"] = $"Update this active account is {state} failed. Server error!!!";
            }
            else
            {
                TempData["Success"] = "Update active account is "+ state+" successfully";
            }
            return RedirectToAction("UserManager");
        }

    }

}
