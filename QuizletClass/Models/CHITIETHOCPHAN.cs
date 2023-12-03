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
        public int LearningModuleDetailId { get; set; }
        [ForeignKey("MA_LOP")]
        public virtual LOP lop { get; set; } = null!;

        [ForeignKey("MA_HOCPHAN")]
        [Column("MA_HOCPHAN")]
        public int LearningModuleId { get; set; }
        [Column("NGAYTAO")]
        public DateTime CreatedDate { get; set; }

    }
}
