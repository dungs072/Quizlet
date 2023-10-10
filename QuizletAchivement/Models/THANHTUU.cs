using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizletAchivement.Models
{
    [Table("THANHTUU", Schema = "dbo")]
    public class THANHTUU
    {
        [Key]
        [Column("MA_THANHTUU")]
        public string AchivementId { get; set; }
        [Column("TENTHANHTUU")]
        public string AchivementName { get; set; }
        [Column("MOTA")]
        public string Describe { get; set; }
        public ICollection<CHITIETTHANHTUU> chitietthanhtuu { get; set; }
    }
}
