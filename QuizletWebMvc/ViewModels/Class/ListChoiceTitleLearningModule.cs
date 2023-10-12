using QuizletWebMvc.ViewModels.Terminology;

namespace QuizletWebMvc.ViewModels.Class
{
    public class ListChoiceTitleLearningModule
    {
        public List<TitleChoiceViewModel> TitleChoiceViewModels { get; set; }
    }
    public class TitleChoiceViewModel
    {
        public TitleViewModel TitleViewModel { get; set; }
        public List<LearningModuleViewModel> LearningModules { get; set; }
    }
    
}
