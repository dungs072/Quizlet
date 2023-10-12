using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using QuizletClass.Models;

namespace QuizletClass.ViewModels
{
    public class ClassViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string? Describe { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public int NumberParticipants { get; set; }
        public int NumberLearningModules { get; set; }

        public void Copy(LOP lop)
        {
            ClassId = lop.ClassId;
            ClassName = lop.ClassName;
            Describe = lop.Describe;
            CreatedDate = lop.CreatedDate;
            UserId = lop.UserId;
        }
    }
}
