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
        public int AchivementDetailId { get; set; }
        [ForeignKey("MA_USER")]
        public int UserId { get; set; }
        //public NGUOIDUNG nguoiDung { get; set; }
        [ForeignKey("MA_THANHTUU")]
        public string AchivementId { get; set; }
        //public THANHTUU thanhTUU { get; set; }
        [Column("NGAYDAT")]
        public DateTime AchieveDate { get; set; }

    }
}
