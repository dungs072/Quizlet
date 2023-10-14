using QuizletClass.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletClass.ViewModels
{
    public class Participant
    {
        public int RegisterDetailClassId { get; set; }
        public int ClassId { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsAccepted { get; set; }
        public int UserId { get; set; }
        public string Gmail { get; set; } = "";
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void Copy(CHITIETDANGKILOP ctdkl)
        {
            RegisterDetailClassId = ctdkl.RegisterDetailClassId;
            ClassId = ctdkl.ClassId;
            RegisterDate = ctdkl.RegisterDate;
            IsAccepted = ctdkl.IsAccepted;
            UserId = ctdkl.UserId;
        }
    }
    public class UserParticipant
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gmail { get; set; }
        public void Copy(NGUOIDUNG nguoidung) 
        {
            UserId = nguoidung.UserId;
            FirstName = nguoidung.FirstName;
            LastName = nguoidung.LastName;
            Gmail = nguoidung.Gmail; 
        }
    }
}
