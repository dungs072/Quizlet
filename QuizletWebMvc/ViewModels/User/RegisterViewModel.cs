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


        public string GmailCode { get; set; }
        [Required(ErrorMessage = "Confirm Gmail Code is required")]
        public string ConfirmGmailCode { get; set; }
        public string TempPass { get; set; }

    }
}
