using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using QuizletWebAPI.Models;

namespace QuizletWebAPI.DBContexts
{
    public class NGUOIDUNGDbContext : DbContext
    {
        public NGUOIDUNGDbContext(DbContextOptions<NGUOIDUNGDbContext> dbContextOptions) : base(dbContextOptions)
        {
            //try
            //{
            //    var databaseCreator = Database.GetService<IDatabaseCreator>()
            //}
        }

        public DbSet<NGUOIDUNG> nGUOIDUNGs { get; set; }
    }
}
