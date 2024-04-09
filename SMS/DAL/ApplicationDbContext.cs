using Microsoft.EntityFrameworkCore;
using SMS.Models;

namespace SMS.DAL
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Students> Students { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<ItemCounts> itemCounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemCounts>(entity =>
            {
                entity.ToTable("ItemCounts");

                entity.HasNoKey();

            });


            base.OnModelCreating(modelBuilder);
        }

    }
}
