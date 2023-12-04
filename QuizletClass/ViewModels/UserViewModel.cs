

namespace QuizletClass.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string Gmail { get; set; } = "";
        public string TypeAccount { get; set; } = "";
        public string Password { get; set; } = "";
        public string? Image { get; set; }
    }
}
