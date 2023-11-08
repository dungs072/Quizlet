using Microsoft.EntityFrameworkCore;
using QuizletTerminology.Models;

namespace QuizletTerminology.DBContexts
{
    public class TerminologyDBContext:DbContext
    {
        public TerminologyDBContext(DbContextOptions<TerminologyDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<CHUDE> chudes { get; set; }
        public DbSet<HOCPHAN> hocphans { get; set; }
        public DbSet<THETHUATNGU> thethuatngus { get;set; }
        public DbSet<NGUOIDUNG> nguoidungs { get; set; }
        public DbSet<LEVELGHINHO> levelghinhos { get; set; }
    }
}
