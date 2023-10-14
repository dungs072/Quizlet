using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.Class
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
    }
    public class ListParticipant
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string Describe { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<Participant> Participants { get; set; }
        public void Copy(ClassViewModel model)
        {
            ClassId = model.ClassId;
            ClassName = model.ClassName;
            Describe = model.Describe;
            CreatedDate = model.CreatedDate;
        }
    }
    public class UserParticipant
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gmail { get; set; }
    }
    public class ListUserParticipant
    {
        public string Search { get; set; } = "~";
        public int ClassId { get; set; }
        public List<UserParticipant> UserParticipants { get; set; }
    }
    public class RegisterDetailClass
    {
        public int RegisterDetailClassId { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsAccepted { get; set; }
    }
}
