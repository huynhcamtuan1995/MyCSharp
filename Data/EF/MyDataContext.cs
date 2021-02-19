using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataSql.Models;
using Microsoft.AspNetCore.Http;
using BaseDataFactory.EF;

namespace DataSql.EF
{
    public class MyDataContext : DataContext
    {
        public MyDataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public MyDataContext(DbContextOptions<DataContext> options, IConfiguration configuration, IHttpContextAccessor httpContext) : base(options)
        {
          
            Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

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

        

            ////seeding data
            //modelBuilder.Seed();
        }
    }

}
