using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public class LearningModuleDetail
    {
        public int LearningModuleDetailId { get; set; }
        public int ClassId { get; set; }
        public int LearningModuleId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
