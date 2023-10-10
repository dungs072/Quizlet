using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.User
{
    public class RegisterViewModel : UserAccountViewModel
    {

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }

    }
}
