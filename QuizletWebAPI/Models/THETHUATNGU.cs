using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletWebAPI.Models
{
    [Table("THETHUATNGU", Schema = "dbo")]
    public class THETHUATNGU
    {
        [Key]
        [Column("MA_THUATNGU")]
        public string MA_THUATNGU { get; set; }
        [Column("TENTHUATNGU")]
        public string TENTHUATNGU { get; set; }
        [Column("GIAITHICH")]
        public string GIAITHICH { get; set; }
        [Column("HINHANH")]
        public string HINHANH { get; set; }
        [ForeignKey("LEVELGHINHO")]
        [Column("MA_LEVEL")]
        public string MA_LEVEL { get; set; }
        public LEVELGHINHO levelGHINHO { get; set; }
        [ForeignKey("HOCPHAN")]
        [Column("MA_HOCPHAN")]
        public string MA_HOCPHAN { get; set; }
        public HOCPHAN hocPHAN { get; set; }
    }
}
