using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletWebAPI.Models
{
    [Table("CHUDE", Schema = "dbo")]
    public class CHUDE
    {
        [Key]
        [Column("MA_CHUDE")]
        public int TitleId { get; set; }
        [Column("TEN_CHU_DE")]
        public string TitleName { get; set; }
        [Column("MOTA")]
        public string Describe { get; set; }
        [ForeignKey("NGUOIDUNG")]
        [Column("MA_USER")]
        public int UserId { get; set; }
        public NGUOIDUNG nguoiDUNG { get; set; }
        public ICollection<HOCPHAN> hocphan { get; set; }
    }
}
