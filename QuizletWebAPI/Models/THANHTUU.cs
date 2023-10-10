using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizletWebAPI.Models
{
    [Table("THANHTUU", Schema = "dbo")]
    public class THANHTUU
    {
        [Key]
        [Column("MA_THANHTUU")]
        public string MA_THANHTUU { get; set; }
        [Column("TENTHANHTUU")]
        public string TENTHANHTUU { get; set; }
        [Column("MOTA")]
        public string MOTA { get; set; }
        public ICollection<CHITIETTHANHTUU> chitietthanhtuu { get; set; }
    }
}
