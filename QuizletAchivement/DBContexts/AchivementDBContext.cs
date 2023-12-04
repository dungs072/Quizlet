
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
        public DbSet<THANHTUU> thanhtuus { get; set; }
    }
}
