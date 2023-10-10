using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletWebAPI.Models
{
    [Table("LEVELGHINHO", Schema = "dbo")]
    public class LEVELGHINHO
    {
        [Key]
        [Column("MA_LEVEL")]
        public string MA_LEVEL { get; set; }
        [Column("TEN_LEVEL")]
        public string TEN_LEVEL { get; set; }
        [Column("MOTA")]
        public string MOTA { get; set; }

        public ICollection<THETHUATNGU> thethuatngu { get; set; }
    }
}
