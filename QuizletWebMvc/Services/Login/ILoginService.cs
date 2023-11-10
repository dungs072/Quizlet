using QuizletWebMvc.ViewModels.User;

namespace QuizletWebMvc.Services.Login
{
    public interface ILoginService
    {

        Task<UserAccountViewModel> FindAccount(string username, string password);
        Task RegisterUser(UserAccountViewModel userAccountView);
        Task<bool> HasDuplicateGmail(string gmail);
        Task<UserAccountViewModel> GetProfile(int userId);
        Task<bool> UpdateProfile(UserAccountViewModel user);
        Task<bool> ChangePassword(ChangePasswordViewModel model);
        Task<string> GetEmailCode(string email);
        Task<bool> HandleForgetPassword(ForgetPasswordViewModel model);
    }
}
