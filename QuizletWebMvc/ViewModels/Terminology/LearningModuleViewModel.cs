using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.Terminology
{
    public class LearningModuleViewModel2
    {
        public int LearningModuleId { get; set; }
        [Display(Name = "Learning module name")]
        [Required(ErrorMessage = "Learning module name is required")]
        public string LearningModuleName { get; set; }
        public string? Describe { get; set; }
        public int TitleId { get; set; }

        public TitleViewModel TitleView { get; set; }
    }
}
