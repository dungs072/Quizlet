using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletTerminology.Models
{
    [Table("HOCPHAN", Schema = "dbo")]
    public class HOCPHAN
    {
        [Key]
        [Column("MA_HOCPHAN")]
        public int LearningModuleId { get; set; } = -1;
        [Column("TEN_HOCPHAN")]
        public string LearningModuleName { get; set; } = "";
        [Column("MOTA")]
        public string? Describe { get; set; }
        [ForeignKey("CHUDE")]
        [Column("MA_CHUDE")]
        public int TitleId { get; set; } = -1;
        [NotMapped]
        public int NumberTerms { get; set; }
        //public CHUDE chuDe { get; set; }
        //public ICollection<CHITIETHOCPHAN> chitiethocphan { get; set; }
    }
}
