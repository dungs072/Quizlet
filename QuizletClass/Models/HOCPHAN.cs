using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletClass.Models
{
    [Table("HOCPHAN", Schema = "dbo")]
    public class HOCPHAN
    {
        [Key]
        [Column("MA_HOCPHAN")]
        public int LearningModuleId { get; set; }
        [Column("TEN_HOCPHAN")]
        public string LearningModuleName { get; set; } = "";
        [Column("MOTA")]
        public string? Describe { get; set; }
        [ForeignKey("MA_CHUDE")]
        public virtual CHUDE chude { get; set; } = null!;
   
        //public virtual ICollection<CHITIETHOCPHAN> chitiethocphans { get; set; }
        public virtual ICollection<THETHUATNGU> thethuatngus { get; set; }


    }
}
