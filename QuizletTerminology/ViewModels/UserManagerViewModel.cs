using QuizletTerminology.Models;

namespace QuizletTerminology.ViewModels
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
        public void Copy(NGUOIDUNG nguoidung)
        {
            UserId = nguoidung.UserId;
            LastName = nguoidung.LastName;
            FirstName = nguoidung.FirstName;
            Gmail = nguoidung.Gmail;
            TypeAccount = nguoidung.TypeAccount;
            Image = nguoidung.Image;
            State = nguoidung.State;
        }
    }
    public class UserState
    {
        public int UserId { get; set; }
        public bool State { get; set; }
    }
}
