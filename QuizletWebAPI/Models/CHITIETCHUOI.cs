﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletWebAPI.Models
{
    [Table("CHITIETCHUOI", Schema = "dbo")]
    public class CHITIETCHUOI
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CTC")]
        public int ID_CTC { get; set; }
        [Column("NGAYHOC")]
        public DateTime NGAYHOC { get; set; }
        [ForeignKey("NGUOIDUNG")]
        [Column("MA_USER")]
        public int MA_USER { get; set; }
        public NGUOIDUNG nguoiDung { get; set; }
    }
}
