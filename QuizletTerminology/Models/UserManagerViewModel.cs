namespace QuizletTerminology.Models
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
            this.UserId = nguoidung.UserId; 
            this.LastName = nguoidung.LastName;
            this.FirstName  = nguoidung.FirstName; 
            this.Gmail = nguoidung.Gmail;
            this.TypeAccount = nguoidung.TypeAccount;
            this.Image = nguoidung.Image;
            this.State = nguoidung.State;
        }
    }
    public class UserState
    {
        public int UserId { get; set; }
        public bool State { get; set; }
    }
}
