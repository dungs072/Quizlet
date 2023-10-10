using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletWebAPI.Models
{
    [Table("LOP", Schema = "dbo")]
    public class LOP
    {
        [Key]
        [Column("MA_LOP")]
        public string MA_LOP { get; set; }
        [Column("TENLOP")]
        public string TENLOP { get; set; }
        [Column("MOTA")]
        public string MOTA { get; set; }
        [ForeignKey("NGUOIDUNG")]
        [Column("MA_USER")]
        public int MA_USER { get; set; }
        public NGUOIDUNG nguoiDUNG { get; set; }

        public ICollection<CHITIETHOCPHAN> chitiethocphan { get; set; }
        public ICollection<CHITIETDANGKILOP> chitietdangkilop { get; set; }
    }
}
