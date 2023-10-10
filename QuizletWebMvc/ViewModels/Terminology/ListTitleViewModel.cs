using QuizletWebMvc.ViewModels.User;

namespace QuizletWebMvc.ViewModels.Terminology
{
    public class ListTitleViewModel
    {
        public UserAccountViewModel User { get; set; }
        public IEnumerable<TitleViewModel> Titles { get; set; }
    }
}
