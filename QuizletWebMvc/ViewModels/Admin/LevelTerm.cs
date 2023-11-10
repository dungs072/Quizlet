using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.Admin
{
    public class LevelTerm
    {
        public int LevelId { get; set; }
        [Required]
        public string LevelName { get; set; }
        public string? Describe { get; set; }
        [RegularExpression("^[0-9]+$", ErrorMessage = "Condition must be a number.")]
        public int Condition { get; set; }
    }
}
