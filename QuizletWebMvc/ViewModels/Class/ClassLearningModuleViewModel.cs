namespace QuizletWebMvc.ViewModels.Class
{
    public class ClassLearningModuleViewModel
    {
        public int LearningModuleId { get; set; }
        public string LearningModuleName { get; set; }
        public string Describe { get; set; }
        public DateTime AddedDate { get; set; }
        public int NumberTerms { get; set; }
    }
}
