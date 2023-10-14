using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.Class
{
    public class LearningModuleViewModel
    {
        public int LearningModuleId { get; set; }
        public string LearningModuleName { get; set; }
        public string? Describe { get; set; }
        public int TitleId { get; set; }
        public bool IsChoose { get; set; }
    }
    public class ListLearningModuleViewModel
    {
        public int TitleId { get; set; }
        public List<LearningModuleViewModel> Modules { get; set; }
    }
}
