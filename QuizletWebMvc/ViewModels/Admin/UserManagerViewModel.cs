using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.Admin
{
    public class UserManagerViewModel
    {
        public int UserId { get; set; } = 0;
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gmail { get; set; }
        public string TypeAccount { get; set; }
        public string? Image { get; set; }
        public bool State { get; set; }
    }

    public class UserState
    {
        public int UserId { get; set; }
        public bool State { get; set; }
    }
}
