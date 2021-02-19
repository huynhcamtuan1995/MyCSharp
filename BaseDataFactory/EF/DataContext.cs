using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using BaseDataFactory.Models;

namespace BaseDataFactory.EF
{
    public partial class DataContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        protected readonly IHttpContextAccessor _httpContext;
       
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration, IHttpContextAccessor httpContext) : base(options)
        {
            _configuration = configuration;
            _httpContext = httpContext;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Username)
                    .IsRequired();
            });
        }


        public virtual int SaveChanges()
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

        
    }

}
