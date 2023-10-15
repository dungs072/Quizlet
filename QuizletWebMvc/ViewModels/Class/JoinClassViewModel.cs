using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.Class
{
    public class JoinClassViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string? Describe { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public int NumberLearningModules { get; set; } = 0;
    }
}
