namespace QuizletWebMvc.ViewModels.Terminology
{
    public class ListTermViewModel
    {
        public LearningModuleViewModel LearningModuleViewModel { get; set; }
        public IEnumerable<TermViewModel> Terms { get; set; }
    }
}
