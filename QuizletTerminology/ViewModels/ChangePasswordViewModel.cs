using System.ComponentModel.DataAnnotations;

namespace QuizletTerminology.ViewModels
{
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class ForgetPasswordViewModel
    {
        public string Email { get; set; }

    }
    public class ChangeGmailViewModel
    {
        public int UserId { get; set; }
        public string GmailAddress { get; set; }    
    }
}
