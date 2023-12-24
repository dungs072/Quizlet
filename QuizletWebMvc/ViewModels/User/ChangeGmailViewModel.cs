using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.User
{
    public class ChangeGmailViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Gmail { get; set; }


        public string OldGmail { get; set; }

        public string GmailCode { get; set; }
        public string RightGmailCode { get; set; }
    }
}
