using QuizletClass.Models;

namespace QuizletClass.ViewModels
{
    public class RegisterClassDetail
    {
        public int RegisterDetailClassId { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsAccepted { get; set; }

        public void Copy(CHITIETDANGKILOP ctdkl)
        {
            RegisterDetailClassId = ctdkl.RegisterDetailClassId;
            UserId = ctdkl.UserId;
            ClassId = ctdkl.lop.ClassId;
            RegisterDate = ctdkl.RegisterDate;
            IsAccepted = ctdkl.IsAccepted;
        }
    }
}
