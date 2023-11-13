using QuizletWebMvc.ViewModels.Class;

namespace QuizletWebMvc.ViewModels.Terminology
{
    public class ListLearningModuleViewModel
    {
        public IEnumerable<LearningModuleViewModel2> Modules { get; set; }

        
        public TitleViewModel TitleViewModel { get; set; }
    }
}
