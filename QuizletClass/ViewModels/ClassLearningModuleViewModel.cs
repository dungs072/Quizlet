using QuizletClass.Models;

namespace QuizletClass.ViewModels
{
    public class ClassLearningModuleViewModel
    {
        public int LearningModuleId { get; set; }
        public string LearningModuleName { get; set; }
        public string Describe { get; set; }
        public DateTime AddedDate { get; set; }
        public int NumberTerms { get; set; }
    }
    public class LearningModuleIdList
    {
        public List<int> Ids { get; } = new List<int>();
        public List<LearningModuleClass> CreatedDates { get; } = new List<LearningModuleClass>();
    }
    public class LearningModuleClass
    {
        public int Ids { get; set; }
        public DateTime CreatedDates { get; set; }
    }
}
