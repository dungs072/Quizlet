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
        [Column("HO")]
        public string LastName { get; set; } = "";
        [Column("TEN")]
        public string FirstName { get; set; } = "";
        [Column("GMAIL")]
        public string Gmail { get; set; } = "";
        [Column("KIEU")]
        public string TypeAccount { get; set; } = "";
        [Column("MATKHAU")]
        public string Password { get; set; } = "";

        public virtual ICollection<LOP> lops { get; set; } 
        public virtual ICollection<CHUDE> chudes { get; set; }

    }
}
