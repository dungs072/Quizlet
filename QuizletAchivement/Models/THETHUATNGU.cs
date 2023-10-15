﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizletAchivement.Models
{
    [Table("THETHUATNGU", Schema = "dbo")]
    public class THETHUATNGU
    {
        [Key]
        [Column("MA_THUATNGU")]
        public int TermId { get; set; }
        [Column("TENTHUATNGU")]
        public string TermName { get; set; }
        [Column("GIAITHICH")]
        public string Explaination { get; set; }
        [Column("HINHANH")]
        public string? Image { get; set; }
        [ForeignKey("MA_HOCPHAN")]
        public HOCPHAN hocphan { get; set; } = new HOCPHAN();

        [ForeignKey("MA_LEVEL")]
        public LEVELGHINHO levelghinho { get; set; } = new LEVELGHINHO();


    }
}