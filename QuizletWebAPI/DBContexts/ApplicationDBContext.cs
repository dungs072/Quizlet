using Microsoft.EntityFrameworkCore;
using QuizletWebAPI.Models;

namespace QuizletWebAPI.DBContexts
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<CHITIETCHUOI> chitietchuois { get; set; }
        public DbSet<CHITIETDANGKILOP> chitietdangkilops { get; set; }
        public DbSet<CHITIETHOCPHAN> chitiethocphans { get; set; }
        public DbSet<CHITIETTHANHTUU> chitietthanhtuus { get; set; }
        public DbSet<CHUDE> chudes { get; set; }
        public DbSet<HOCPHAN> hocphans { get; set; }
        public DbSet<LEVELGHINHO> levelghinhos { get; set; }
        public DbSet<LOP> lops { get; set; }
        public DbSet<NGUOIDUNG> nguoidungs { get; set; }
        public DbSet<THANHTUU> thanhtuus { get; set; }
        public DbSet<THETHUATNGU> thethuatngus { get; set; }
    }
}
