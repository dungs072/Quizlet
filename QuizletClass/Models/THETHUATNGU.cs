using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizletClass.Models
{
    [Table("THETHUATNGU", Schema = "dbo")]
    public class THETHUATNGU
    {
        [Key]
        [Column("MA_THUATNGU")]
        public int TermId { get; set; }
        [Column("TENTHUATNGU")]
        public string TermName { get; set; }
        [Column("GIAITHICH")]
        public string Explaination { get; set; }
        [Column("HINHANH")]
        public string? Image { get; set; }
        [ForeignKey("HOCPHAN")]
        [Column("MA_HOCPHAN")]
        public int LearningModuleId { get; set; }

        [ForeignKey("LEVEL")]
        [Column("MA_LEVEL")]
        public int LevelId { get; set; } = 1;
        [Column("TICHLUY")]
        public int AC { get; set; }

        //public HOCPHAN hocPHAN { get; set; }
    }
}
