using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletClass.Models
{
    [Table("CHITIETDANGKILOP", Schema = "dbo")]
    public class CHITIETDANGKILOP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CTL")]
        public int ID_CTL { get; set; }
        [ForeignKey("NGUOIDUNG")]
        [Column("MA_USER")]
        public int MA_USER { get; set; }
        public NGUOIDUNG nguoiDUNG { get; set; }
        [ForeignKey("LOP")]
        [Column("MA_LOP")]
        public string MA_LOP { get; set; }
        public LOP lop { get; set; }
        [Column("NGAYDANGKI")]
        public DateTime NGAYDANGKI { get; set; }
        [Column("CHAPTHUAN")]
        public bool CHAPTHUAN { get; set; }
    }
}
