
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletAchivement.Models
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
        [ForeignKey("MA_CHUDE")]
        public virtual CHUDE chude { get; set; }
   
        public virtual ICollection<THETHUATNGU> thethuatngus { get; set; } = new List<THETHUATNGU>();

    }
}
