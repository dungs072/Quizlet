using Microsoft.AspNetCore.Mvc.Rendering;
using QuizletWebMvc.ViewModels.Achivement;
using QuizletWebMvc.ViewModels.Terminology;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletWebMvc.ViewModels.User
{
    [Serializable]
    public class UserAccountViewModel
    {
        public int UserId { get; set; } = 0;
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Gmail { get; set; }
        [Display(Name = "Type account")]
        [Required(ErrorMessage = "Type account is required")]
        public string TypeAccount { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public List<LevelTerms> LevelTerms { get; set; }


        public override string ToString()
        {
            return UserId.ToString();
        }
        public List<SelectListItem> SelectedRole 
        {
            get 
            {
                if(TypeAccount=="Admin")
                {
                    return new List<SelectListItem>
                    {
                    new SelectListItem { Text = "Admin", Value = "Admin" },
                    };
                }
                else
                {
                    return new List<SelectListItem>
                    {
                    new SelectListItem { Text = "Teacher", Value = "Teacher" },
                    new SelectListItem { Text = "Student", Value = "Student" }
                    };
                }
               
            } 
        }



    }
}
