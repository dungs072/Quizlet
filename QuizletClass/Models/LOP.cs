using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletClass.Models
{
    [Table("LOP", Schema = "dbo")]
    public class LOP
    {
        [Key]
        [Column("MA_LOP")]
        public string ClassId { get; set; }
        [Column("TENLOP")]
        public string ClassName { get; set; }
        [Column("MOTA")]
        public string Describe { get; set; }
        [ForeignKey("NGUOIDUNG")]
        [Column("MA_USER")]
        public int UserId { get; set; }

        public ICollection<CHITIETHOCPHAN> chitiethocphan { get; set; }
        public ICollection<CHITIETDANGKILOP> chitietdangkilop { get; set; }
    }
}
