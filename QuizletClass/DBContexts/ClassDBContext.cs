
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuizletClass.Models;
using System.Reflection.Metadata;

namespace QuizletClass.DBContexts
{
    public class ClassDBContext:DbContext
    {
        public ClassDBContext(DbContextOptions<ClassDBContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<LOP>()
            //.Property<int>("MA_USER");
            

            //modelBuilder.Entity<LOP>()
            //            .HasMany(e => e.chitietdangkilop)
            //            .WithOne(e => e.lop)
            //            .HasForeignKey("MA_LOP")
            //            .IsRequired();
            //modelBuilder.Entity<LOP>()
            //            .HasMany(e => e.chitiethocphan)
            //            .WithOne(e => e.lop)
            //            .HasForeignKey("MA_LOP")
            //            .IsRequired();

        }
        public DbSet<CHITIETDANGKILOP> chitietdangkilops { get; set; }
        public DbSet<CHITIETHOCPHAN> chitiethocphans { get; set; }
        public DbSet<NGUOIDUNG> nguoidungs { get; set; }
        public DbSet<LOP> lops { get; set; }
        public DbSet<HOCPHAN> hocphans { get; set; }
        public DbSet<THETHUATNGU> thethuatngus { get; set; }
        public DbSet<CHUDE> chudes { get; set; }
    }
}
