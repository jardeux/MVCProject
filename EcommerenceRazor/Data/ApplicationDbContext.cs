using Microsoft.EntityFrameworkCore;

namespace EcommerenceRazor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Models.Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Category>().HasData(
                new Models.Category { CategoryId = 1, Name = "Action", DisplayOrder = 1 },
                new Models.Category { CategoryId = 2, Name = "Adventure", DisplayOrder = 2 },
                new Models.Category { CategoryId = 3, Name = "RPG", DisplayOrder = 3 }
            );
        }
    }
}
