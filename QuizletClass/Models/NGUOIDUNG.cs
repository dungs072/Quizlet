using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletClass.Models
{
    [Table("NGUOIDUNG", Schema = "dbo")]
    public class NGUOIDUNG
    {
        [Key]
        [Column("MA_USER")]
        public int UserId { get; set; }

    }
}
