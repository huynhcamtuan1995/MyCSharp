using DataSql.Models;
using Microsoft.EntityFrameworkCore;

namespace DataSql.EF
{
    public static class Seeding
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { ID = 1, Name = "Văn Phòng Phẩm" },
                new Category { ID = 2, Name = "Đồ chơi" }
                );

            modelBuilder.Entity<Product>().HasData(
                new { ID = 1, Name = "Bút Thiên Long", CategoryID = 1 },
                new { ID = 2, Name = "Gấu nhồi bông", Quantity = 2, CategoryID = 2 },
                new { ID = 3, Name = "Siêu nhân gao", Quantity = 10, CategoryID = 2 }
                );
        }
    }
}
