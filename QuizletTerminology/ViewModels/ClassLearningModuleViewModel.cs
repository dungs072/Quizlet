namespace QuizletTerminology.ViewModels
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
        public List<int> Ids { get; set; }
        public List<LearningModuleClass> CreatedDates { get; set; }
    }
    public class LearningModuleClass
    {
        public int Ids { get; set; }
        public DateTime CreatedDates { get; set; }
    }
    public class CopyViewModel
    {
        public int TitleId { get; set; }
        public int ModuleId { get; set; }
    }

}
