namespace QuizletClass.ViewModels
{
    public class ListLearningModuleViewModel
    {
        public string TitleId { get; set; }
        public string TitleName { get; set; }
        public string? Describe { get; set; }
        public List<TermViewModel> Modules { get; set; }
    }
}
