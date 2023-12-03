using System.ComponentModel.DataAnnotations;

namespace QuizletClass.ViewModels
{
    public class LearningModuleViewModel2
    {
        public int LearningModuleId { get; set; }
        public string LearningModuleName { get; set; }
        public string? Describe { get; set; }
        public int TitleId { get; set; }

        public int NumberTerms { get; set; }

    }
}
