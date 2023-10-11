using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Login;
using QuizletWebMvc.ViewModels.User;

namespace WebMVCQuizlet.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService loginService;
        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public IActionResult LogOut()
        {
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
            
            if (user != null&&user.UserId!=0)
            {
              
                HttpContext.Session.SetString("UserId", user.ToString());
                HttpContext.Session.SetString("UserName", user.LastName + " " + user.FirstName);
                HttpContext.Session.SetString("TypeUser", user.TypeAccount);
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
            if (!ModelState.IsValid) return View(registerViewModel);
            var isDuplicated = await loginService.HasDuplicateGmail(registerViewModel.Gmail);
            if (isDuplicated)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }
            UserAccountViewModel user = registerViewModel;
            await loginService.RegisterUser(user);

            return RedirectToAction("Index", "Login");
        }
    }
}
