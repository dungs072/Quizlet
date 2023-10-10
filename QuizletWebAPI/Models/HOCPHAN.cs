using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletWebAPI.Models
{
    [Table("HOCPHAN", Schema = "dbo")]
    public class HOCPHAN
    {
        [Key]
        [Column("MA_HOCPHAN")]
        public string MA_HOCPHAN { get; set; }
        [Column("TEN_HOCPHAN")]
        public string TEN_HOCPHAN { get; set; }
        [Column("MOTA")]
        public string MOTA { get; set; }
        [ForeignKey("CHUDE")]
        [Column("MA_CHUDE")]
        public string MA_CHUDE { get; set; }
        public CHUDE chuDe { get; set; }
        public ICollection<THETHUATNGU> thethuatngu { get; set; }
        public ICollection<CHITIETHOCPHAN> chitiethocphan { get; set; }
    }
}
