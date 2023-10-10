using QuizletWebMvc.ViewModels.User;

namespace QuizletWebMvc.Services.Login
{
    public interface ILoginService
    {

        Task<UserAccountViewModel> FindAccount(string username, string password);
        Task RegisterUser(UserAccountViewModel userAccountView);
        Task<bool> HasDuplicateGmail(string gmail);
    }
}
