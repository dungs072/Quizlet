using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace QuizletAchivement.Models
{
    [Table("CHITIETTHANHTUU", Schema = "dbo")]
    public class CHITIETTHANHTUU
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CTTT")]
        public int ID_CTTT { get; set; }
        [ForeignKey("NGUOIDUNG")]
        [Column("MA_USER")]
        public int MA_USER { get; set; }
        public NGUOIDUNG nguoiDung { get; set; }
        [ForeignKey("THANHTUU")]
        [Column("MA_THANHTUU")]
        public string MA_THANHTUU { get; set; }
        public THANHTUU thanhTUU { get; set; }
        [Column("NGAYDAT")]
        public DateTime NGAYDAT { get; set; }

    }
}
