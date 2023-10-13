
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuizletClass.Models;

namespace QuizletClass.DBContexts
{
    public class ClassDBContext:DbContext
    {
        public ClassDBContext(DbContextOptions<ClassDBContext> dbContextOptions) : base(dbContextOptions)
        {
           
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
