﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletClass.Models
{
    [Table("CHITIETDANGKILOP", Schema = "dbo")]
    public class CHITIETDANGKILOP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CTL")]
        public int RegisterDetailClassId { get; set; }
        [Column("NGAYDANGKI")]
        public DateTime RegisterDate { get; set; }
        [Column("CHAPTHUAN")]
        public bool IsAccepted { get; set; }
        [ForeignKey("MA_USER")]
        [Column("MA_USER")]
        public int UserId { get; set; }
        [ForeignKey("MA_LOP")]
        public virtual LOP lop { get; set; } = null!;

    }
}
