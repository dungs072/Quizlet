
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
        //public int TitleId { get; set; }
        public CHUDE chuDe { get; set; } = new CHUDE();
   
        public ICollection<THETHUATNGU> thethuatngus { get; set; } = new List<THETHUATNGU>();

    }
}
