using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.User
{
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please enter your current password.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please enter a new password.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
