namespace QuizletWebMvc.ViewModels.Terminology
{
    public class ListLearningModuleViewModel
    {
        public IEnumerable<LearningModuleViewModel> Modules { get; set; }
        public TitleViewModel TitleViewModel { get; set; }
    }
}
