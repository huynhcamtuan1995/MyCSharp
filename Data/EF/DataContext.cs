using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataSql.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using DataSql.Constants;
using System.Security.Claims;
using System.Collections.Generic;

namespace DataSql.EF
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration, IHttpContextAccessor httpContext) : base(options)
        {
            _configuration = configuration;
            _httpContext = httpContext;
            Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        public override int SaveChanges()
        {
            UpdateTime();
            return base.SaveChanges();
        }

        private void UpdateTime()
        {
            var currentTime = DateTime.Now;
            int? userId = null;
            if (_httpContext.HttpContext != null)
            {
                Claim identity = ((ClaimsIdentity)_httpContext.HttpContext.User.Identity)?.FindFirst(ClaimTypes.NameIdentifier);
                if(int.TryParse(identity?.Value, out int parseId))
                {
                    userId = parseId;
                }
            }

            //Find all Entities that are Added/Modified that inherit from my EntityBase
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

            foreach (var entry in entries)
            {
                //block input CreatedAt, CreatedBy, ModifiedAt, ModifiedBy
                if (entry.State == EntityState.Added)
                {
                    if (entry.Metadata.FindProperty("CreatedAt") != null)
                    {
                        //if (entry.Property("CreatedAt").CurrentValue == null)
                        entry.Property("CreatedAt").CurrentValue = currentTime;
                    }

                    if (entry.Metadata.FindProperty("CreatedBy") != null)
                    {
                        //if (entry.Property("CreatedBy").CurrentValue == null)
                        entry.Property("CreatedBy").CurrentValue = userId;
                    }
                }

                if (entry.Metadata.FindProperty("ModifiedAt") != null)
                {
                    //if (entry.Property("ModifiedAt").CurrentValue == null)
                    entry.Property("ModifiedAt").CurrentValue = currentTime;
                }

                if (entry.Metadata.FindProperty("ModifiedBy") != null)
                {
                    //if (entry.Property("ModifiedBy").CurrentValue == null)
                    entry.Property("ModifiedBy").CurrentValue = userId;
                }
            }
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
