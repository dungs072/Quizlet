using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizletClass.ViewModels
{
    public class TermViewModel
    {
        public int TermId { get; set; }
        public string TermName { get; set; }
        public string Explaination { get; set; }
        public string? Image { get; set; }
        public int LearningModuleId { get; set; }
        public int LevelId { get; set; } = 1;
    }
}
