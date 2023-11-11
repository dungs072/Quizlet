using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.Terminology
{
    public class TitleViewModel
    {
        public int TitleId { get; set; } = 0;
        [Display(Name = "Title name")]
        [Required(ErrorMessage = "Title name is required")]
        public string TitleName { get; set; } = "";
        public string? Describe { get; set; } = "";
        public int UserId { get; set; } = 0;

        public bool IsEmpty { get; set; }
    }
}
