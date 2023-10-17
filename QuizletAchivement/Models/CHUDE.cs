﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizletAchivement.Models
{
    [Table("CHUDE", Schema = "dbo")]
    public class CHUDE
    {
        [Key]
        [Column("MA_CHUDE")]
        public int TitleId { get; set; }
        [Column("TEN_CHUDE")]
        public string TitleName { get; set; }
        [Column("MOTA")]
        public string? Describe { get; set; }
        [ForeignKey("MA_USER")]
        public virtual NGUOIDUNG nguoidung { get; set; }


        public virtual ICollection<HOCPHAN> hocphans { get; set; }
        
    }
}
