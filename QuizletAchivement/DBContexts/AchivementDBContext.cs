
using Microsoft.EntityFrameworkCore;
using QuizletAchivement.Models;

namespace QuizletAchivement.DBContexts
{
    public class AchivementDBContext:DbContext
    {
        public AchivementDBContext(DbContextOptions<AchivementDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public DbSet<CHITIETCHUOI> chitietchuois { get; set; }
        public DbSet<CHITIETTHANHTUU> chitietthanhtuus { get; set; }
        public DbSet<NGUOIDUNG> nguoidungs { get; set; }
        public DbSet<THANHTUU> thanhtuus { get; set; }
        public DbSet<LEVELGHINHO> levelghinhos { get; set; }
        public DbSet<THETHUATNGU> thethuatngus { get; set; }
        public DbSet<CHUDE> chudes { get; set; }
        public DbSet<HOCPHAN> hocphans { get; set; }
        public DbSet<LOP> lops { get; set; }
    }
}
