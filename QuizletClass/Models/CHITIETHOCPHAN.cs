using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletClass.Models
{
    [Table("CHITIETHOCPHAN", Schema = "dbo")]
    public class CHITIETHOCPHAN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CTHP")]
        public int ID_CTHP { get; set; }
        [ForeignKey("LOP")]
        [Column("MA_LOP")]
        public string MA_LOP { get; set; }
        public LOP lop { get; set; }
        [ForeignKey("HOCPHAN")]
        [Column("MA_HOCPHAN")]
        public string MA_HOCPHAN { get; set; }
        public HOCPHAN hocPHAN { get; set; }
        public DateTime NGAYTAO { get; set; }
    }
}
