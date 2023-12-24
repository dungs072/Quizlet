using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Models;
using QuizletWebMvc.Services.Achivement;
using QuizletWebMvc.Services.Class;
using QuizletWebMvc.Services.Firebase;
using QuizletWebMvc.Services.Login;
using QuizletWebMvc.Services.Token;
using QuizletWebMvc.ViewModels.Achivement;
using QuizletWebMvc.ViewModels.Class;
using QuizletWebMvc.ViewModels.User;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;


namespace QuizletWebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAchivement achivement;
        private readonly ILoginService loginService;
        private readonly IClassService classService;
        private readonly IFirebaseService firebaseService;
        private readonly ITokenService tokenService;

        public HomeController(ILogger<HomeController> logger, IAchivement achivement,ILoginService loginService, 
                        IFirebaseService firebaseService, IClassService classService,ITokenService tokenService)
        {
            _logger = logger;
            this.achivement = achivement;
            this.loginService = loginService;
            this.firebaseService = firebaseService;
            this.classService = classService; 
            this.tokenService = tokenService;
        }
        private bool CheckCurrentToken()
        {
            string token = Request.Cookies["AuthToken"];
            if (token == null) { return false; }
            ClaimsPrincipal principal = tokenService.ValidateToken(token);
            return principal != null;
        }

        public async Task<IActionResult> Index()
        {
            if(!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            UserAchivement userAchivement = new UserAchivement();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                List<LevelTerms> levelTerms = await achivement.GetLevelTerm(userId);
               
                userAchivement.LevelTerms = levelTerms;
            }
            MarkAttendance mark = new MarkAttendance();
            mark.UserId = userId;
            bool canMark = await achivement.MarkAttendance(mark);
            if (canMark)
            {
                TempData["Success"] = "Hello my friend. Your attendance today is marked!!";
            }
            return View(userAchivement);
        }
        [HttpGet]
        public async Task<IActionResult> LoadMoreAchivement(int pageNumber)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId)) { }
            UserAchivement userAchivement = new UserAchivement();
            if(pageNumber==1) 
            {
                AchieveStatistics achieveStatistics = await achivement.GetAchieveStatistics(userId);
                userAchivement.AchieveStatistics = achieveStatistics;
                return PartialView("PartialView1", userAchivement);
            }
            if(pageNumber==2)
            {
                List<string> sequenceDates = await achivement.GetSequenceDates(userId);
                userAchivement.SequenceDates = sequenceDates;
                return PartialView("PartialView2",userAchivement);
            }
            if(pageNumber==3)
            {
                List<Badge> badges = await achivement.GetBadges(userId);
                List<Badge> moduleBadges = new List<Badge>();
                List<Badge> termBadges = new List<Badge>();
                List<Badge> participantBadges = new List<Badge>();
                foreach (var badge in badges)
                {
                    string[] temp = badge.NameBadge.Split(',');
                    badge.NameBadge = temp[0];
                    if (temp[1]=="modules")
                    {
                        moduleBadges.Add(badge);
                    }
                    if (temp[1]=="terms")
                    {
                        termBadges.Add(badge);
                    }
                    if (temp[1]=="participants")
                    {
                        participantBadges.Add(badge);
                    }
                }
                userAchivement.moduleBadges = moduleBadges;
                userAchivement.termBadges = termBadges;
                userAchivement.participantBadges = participantBadges;
                return PartialView("PartialView3", userAchivement);
            }
            return Json(false);
            
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId)) 
            {
                var user = await loginService.GetProfile(userId);
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserAccountViewModel model, IFormFile imageFile, IFormCollection form)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            ModelState.Remove("LevelTerms");
            ModelState.Remove("Image");
            ModelState.Remove("imageFile");
            ModelState.Remove("deleteImage");
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "Error while updating your profile";
                return View(model);
            }
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                model.UserId = userId;
                bool canDelete =  form["deleteImage"]=="on";
                if (canDelete)
                {
                    if (model.Image != null)
                    {
                        await firebaseService.DeleteImage(model.Image, "users");
                        model.Image = null;
                    }
                }
                else
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        if (model.Image != null)
                        {
                            await firebaseService.DeleteImage(model.Image, "users");
                        }
                        model.Image = await firebaseService.StoreImage(imageFile, "users");
                    }
                }
             
              
                var state = await loginService.UpdateProfile(model);
                if(state)
                {
                    TempData["Success"] = "Update profile successfully";
                    HttpContext.Session.SetString("UserName", model.LastName + " " + model.FirstName);
                    HttpContext.Session.SetString("TypeUser", model.TypeAccount);
                    HttpContext.Session.SetString("Image", model.Image == null ? "none" : model.Image);
                    return View(model);
                }

            }
            TempData["Error"] = "Error while update your profile";
            return View(model);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            if (!ModelState.IsValid) { return View(model); }
            if(model.NewPassword!=model.ConfirmPassword)
            {
                TempData["Error"] = "Your new password does not match with confirm password";
                return View(model);
            }
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId)) { }
            model.UserId = userId;
            var state = await loginService.ChangePassword(model);
            if(state)
            {
                TempData["Success"] = "Change password successfully";
            }
            else
            {
                TempData["Error"] = "Your old password does not right";
            }
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> ChangeGmail()
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            ChangeGmailViewModel model = new ChangeGmailViewModel();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                var user = await loginService.GetProfile(userId);
                model.Gmail = user.Gmail;
                model.OldGmail = user.Gmail;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeGmail(ChangeGmailViewModel changeGmailViewModel)
        {
            if (changeGmailViewModel.OldGmail == changeGmailViewModel.Gmail)
            {
                TempData["Error"] = "Error. your email you want to change to is the same with the old one";
                return RedirectToAction("ChangeGmail","Home");
            }
            string gmailCode = await loginService.GetEmailCode(changeGmailViewModel.OldGmail);
            changeGmailViewModel.GmailCode = "";
            changeGmailViewModel.RightGmailCode = gmailCode;
            return View("HomeGmailChecking",changeGmailViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> GmailChecking(ChangeGmailViewModel changeGmailViewModel)
        {
            if (changeGmailViewModel.RightGmailCode != changeGmailViewModel.GmailCode)
            {
                TempData["Error"] = "Error. you enter the wrong 6 digits code";
                return View("HomeGmailChecking", changeGmailViewModel);
            }
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId)) { }
            ChangeGmailViewModel2 model = new ChangeGmailViewModel2();
            model.UserId = userId;
            model.GmailAddress = changeGmailViewModel.Gmail;
            var state = await loginService.ChangeGmail(model);
            if (state)
            {
                TempData["Success"] = "Success. Your email has been changed.\nNew email: " + changeGmailViewModel.Gmail;
            }
            else
            {
                TempData["Error"] = "Your gmail was registered before. Please enter another one";
            }
            
            return RedirectToAction("ChangeGmail");
        }
        public async Task<ActionResult> GetMessages()
        {
            if (!CheckCurrentToken())
            {
                TempData["Error"] = "Error. Please dont intrude to other personality";
                return RedirectToAction("Index", "Login");
            }
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId)) { }
            List<MessageClassRegistration> models = await classService.GetMessageRegister(userId);

            ViewBag.Messages = models;

            return PartialView("_MessageListPartial");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Welcome()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}