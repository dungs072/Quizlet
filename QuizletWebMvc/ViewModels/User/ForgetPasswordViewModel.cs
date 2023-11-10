using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.User
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

    }
}
