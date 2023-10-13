using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.Class
{
    public class LearningModuleViewModel
    {
        public int LearningModuleId { get; set; }
        public string LearningModuleName { get; set; }
        public string? Describe { get; set; }
        public int TitleId { get; set; }
    }
    public class ListLearningModuleViewModel
    {
        public int TitleId { get; set; }
        public IEnumerable<LearningModuleViewModel> Modules { get; set; }
    }
}
