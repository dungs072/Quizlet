using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletAchivement.Models
{
    [Table("CHITIETCHUOI", Schema = "dbo")]
    public class CHITIETCHUOI
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CTC")]
        public int SequenceId { get; set; }
        [Column("NGAYHOC")]
        public DateTime LearningDay { get; set; }
        [ForeignKey("MA_USER")]
        [Column("MA_USER")]
        public int UserId { get; set; }
    }
}
