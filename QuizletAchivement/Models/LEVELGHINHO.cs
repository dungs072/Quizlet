using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletAchivement.Models
{
    [Table("LEVELGHINHO", Schema = "dbo")]
    public class LEVELGHINHO
    {
        [Key]
        [Column("MA_LEVEL")]
        public int LevelId { get; set; }
        [Column("TEN_LEVEL")]
        public string LevelName { get; set; }
        [Column("MOTA")]
        public string? Describe { get; set; }
        [Column("DIEUKIEN")]
        public int Condition { get; set; }

        public ICollection<THETHUATNGU> thethuatngus { get; set; } = new List<THETHUATNGU>();
    }
}
