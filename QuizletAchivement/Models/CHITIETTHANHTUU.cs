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
        public virtual NGUOIDUNG nguoidung { get; set; }
        [ForeignKey("MA_THANHTUU")]
        public virtual THANHTUU thanhtuu { get; set; }
        
        [Column("NGAYDAT")]
        public DateTime AchieveDate { get; set; }

        

    }
}
