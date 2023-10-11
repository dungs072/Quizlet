﻿
using Microsoft.EntityFrameworkCore;
using QuizletAchivement.Models;

namespace QuizletAchivement.DBContexts
{
    public class AchivementDBContext:DbContext
    {
        public AchivementDBContext(DbContextOptions<AchivementDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<CHITIETCHUOI> chitietchuois { get; set; }
        public DbSet<CHITIETTHANHTUU> chitietthanhtuus { get; set; }
        public DbSet<NGUOIDUNG> nguoidungs { get; set; }
        public DbSet<THANHTUU> thanhtuus { get; set; }
    }
}