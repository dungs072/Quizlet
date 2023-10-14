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
        public int RegisterDetailClassId { get; set; }
        [ForeignKey("NGUOIDUNG")]
        [Column("MA_USER")]
        public int UserId { get; set; }
        //public NGUOIDUNG nguoiDUNG { get; set; }
        [ForeignKey("LOP")]
        [Column("MA_LOP")]
        public int ClassId { get; set; }
        //public LOP lop { get; set; }
        [Column("NGAYDANGKI")]
        public DateTime RegisterDate { get; set; }
        [Column("CHAPTHUAN")]
        public bool IsAccepted { get; set; }
    }
}
