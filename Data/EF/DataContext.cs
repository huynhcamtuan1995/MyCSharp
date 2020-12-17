using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Data.Models;

namespace Data.EF
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.HasMany(p => p.Products)
                    .WithOne(c => c.Category)
                    .HasForeignKey(c => c.CategoryID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.Quantity).HasDefaultValue(0);
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(p => p.CategoryID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Username)
                    .IsRequired();
            });

            ////seeding data
            //modelBuilder.Seed();
        }
    }

}
