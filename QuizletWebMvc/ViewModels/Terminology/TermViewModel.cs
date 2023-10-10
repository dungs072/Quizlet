using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletWebMvc.ViewModels.Terminology
{
    public class TermViewModel
    {
        public int TermId { get; set; }
        [Display(Name = "Term name")]
        [Required(ErrorMessage = "Terminology name is required")]
        public string TermName { get; set; }
        [Display(Name = "Term Explanation")]
        [Required(ErrorMessage = "Explanation is required")]
        public string Explaination { get; set; }
        public string? Image { get; set; }
        public int LearningModuleId { get; set; }
        public LearningModuleViewModel LearningModule { get; set; }
    }
}
