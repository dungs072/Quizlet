using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizletAchivement.Models
{
    [Table("THANHTUU", Schema = "dbo")]
    public class THANHTUU
    {
        [Key]
        [Column("MA_THANHTUU")]
        public int AchivementId { get; set; }
        [Column("TENTHANHTUU")]
        public string AchivementName { get; set; }
        [Column("DIEUKIEN")]
        public int Condition { get; set; }
        [Column("HINHANH")]
        public string? Image { get; set; }

        public bool CanDelete { get; set; } = false;

    }
}
