using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Infrastructure.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        #region DbSet for tables

        public DbSet<Quiz> Quizes { get; set; }
        public DbSet<QuizItem> QuizItems { get; set; }
        public DbSet<Student> Students { get; set; }

        #endregion
    }
}