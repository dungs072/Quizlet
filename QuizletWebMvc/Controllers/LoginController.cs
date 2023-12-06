using Firebase.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizletWebMvc.Services.Login;
using QuizletWebMvc.Services.Token;
using QuizletWebMvc.ViewModels.User;

namespace WebMVCQuizlet.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService loginService;
        private readonly ITokenService tokenService;
        public LoginController(ILoginService loginService, ITokenService tokenService)
        {
            this.loginService = loginService;
            this.tokenService = tokenService;
        }

        public IActionResult LogOut()
        {
            Response.Cookies.Delete("AuthToken");

            return RedirectToAction("Welcome", "Home");
        }
        [HttpGet]
        public IActionResult Index()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await loginService.FindAccount(loginViewModel.EmailAddress,loginViewModel.Password);
            if(user!=null && user.UserId==123)
            {
                TempData["Error"] = "Your account is being locked by admin. Please contact to admin for unlock your account !!!";
                return View(loginViewModel);
            }
            if (user != null&&user.UserId!=0)
            {
              
                HttpContext.Session.SetString("UserId", user.ToString());
                HttpContext.Session.SetString("UserName", user.LastName + " " + user.FirstName);
                HttpContext.Session.SetString("TypeUser", user.TypeAccount);
                HttpContext.Session.SetString("Image", user.Image==null?"none":user.Image);

                string token = tokenService.GenerateToken(user.UserId.ToString());
                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                });

                if (user.TypeAccount=="Admin")
                {
                    return RedirectToAction("LevelTerm", "Admin");
                }
                return RedirectToAction("Index", "Home");
            }
            //User not found
            TempData["Error"] = "Wrong user name or password. Please try again !!!";
            return View(loginViewModel);
        }
        [HttpGet]
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            ModelState.Remove("ConfirmGmailCode");
            ModelState.Remove("GmailCode");
            ModelState.Remove("LevelTerms");
            ModelState.Remove("TempPass");
            if (!ModelState.IsValid) return View(registerViewModel);
            var isDuplicated = await loginService.HasDuplicateGmail(registerViewModel.Gmail);
            if (isDuplicated)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }
            registerViewModel.TempPass = registerViewModel.Password;
            return RedirectToAction("GmailChecking", "Login",registerViewModel);

        }
        [HttpGet]
        public async Task<IActionResult> GmailChecking(RegisterViewModel registerViewModel)
        {

            string gmailCode = await loginService.GetEmailCode(registerViewModel.Gmail);
            registerViewModel.GmailCode = gmailCode;
            return View(registerViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> PostGmailChecking(RegisterViewModel registerViewModel)
        {

            ModelState.Remove("GmailCode");
            ModelState.Remove("LevelTerms");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Password");
            ModelState.Remove("TempPass");
            if (!ModelState.IsValid) return View("GmailChecking", registerViewModel);
            if(registerViewModel.ConfirmGmailCode!=registerViewModel.GmailCode)
            {
                TempData["Error"] = "Confirm gmail code does not match";
                return View("GmailChecking", registerViewModel);
            }
            registerViewModel.Password = registerViewModel.TempPass;
            UserAccountViewModel user = registerViewModel;
            await loginService.RegisterUser(user);
            TempData["Success"] = "Create new account successfully";
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            ForgetPasswordViewModel forgetPasswordViewModel = new ForgetPasswordViewModel();
            return View(forgetPasswordViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }
            bool state = await loginService.HandleForgetPassword(model);
            if(state)
            {
                TempData["Success"] = "Successfully, reset password";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["Error"] = "Error, Your email you enter is not registered in Quizlet";
                return View(model);
            }
        }

    }
}
