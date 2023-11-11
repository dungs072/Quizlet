using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletTerminology.Models
{
    [Table("CHUDE", Schema = "dbo")]
    public class CHUDE
    {
        [Key]
        [Column("MA_CHUDE")]
        public int TitleId { get; set; }
        [Column("TEN_CHUDE")]
        public string TitleName { get; set; }
        [Column("MOTA")]
        public string? Describe { get; set; }
        [ForeignKey("NGUOIDUNG")]
        [Column("MA_USER")]
        public int UserId { get; set; }

        [NotMapped]
        public bool IsEmpty { get; set; } = true;
        //public NGUOIDUNG nguoiDUNG { get; set; }
    }
}
