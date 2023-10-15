namespace QuizletWebMvc.ViewModels.Terminology
{
    public class ListTermViewModel
    {
        public LearningModuleViewModel2 LearningModuleViewModel { get; set; }
        public IEnumerable<TermViewModel> Terms { get; set; }

        public string GlobalSearch { get; set; }

        public string LearningModuleName { get; set; }

        public int ClassId { get; set; }  
    }
}
