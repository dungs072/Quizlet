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
        [ForeignKey("LOP")]
        [Column("MA_LOP")]
        public int ClassId { get; set; }
        
        [ForeignKey("HOCPHAN")]
        [Column("MA_HOCPHAN")]
        public int LearningModuleId { get; set; }
        [Column("NGAYTAO")]
        public DateTime CreatedDate { get; set; }

        public LOP Class { get; set; }
        public HOCPHAN LearningModule { get; set; }

    }
}
