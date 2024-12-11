

using Mango.Services.CategoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Mango.Services.CategoryAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Kimchi",
                    ImageUrl = "https://localhost:7005/CategoryImages/1.png",
                    ImageLocalPath = @"wwwroot\CategoryImages\1.png"
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Jjigae",
                    ImageUrl = "https://localhost:7005/CategoryImages/2.png",
                    ImageLocalPath = @"wwwroot\CategoryImages\2.png"
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Banchan",
                    ImageUrl = "https://localhost:7005/CategoryImages/3.png",
                    ImageLocalPath = @"wwwroot\CategoryImages\3.png"
                },
                new Category
                {
                    CategoryId = 4,
                    CategoryName = "Bulgogi and Gui",
                    ImageUrl = "https://localhost:7005/CategoryImages/4.png",
                    ImageLocalPath = @"wwwroot\CategoryImages\4.png"
                },
                new Category
                {
                    CategoryId = 5,
                    CategoryName = "Tteok",
                    ImageUrl = "https://localhost:7005/CategoryImages/5.png",
                    ImageLocalPath = @"wwwroot\CategoryImages\5.png"
                },
                new Category
                {
                    CategoryId = 6,
                    CategoryName = "Jeon",
                    ImageUrl = "https://localhost:7005/CategoryImages/6.png",
                    ImageLocalPath = @"wwwroot\CategoryImages\6.png"
                },
                new Category
                {
                    CategoryId = 7,
                    CategoryName = "Guksu",
                    ImageUrl = "https://localhost:7005/CategoryImages/7.png",
                    ImageLocalPath = @"wwwroot\CategoryImages\7.png"
                },
                new Category
                {
                    CategoryId = 8,
                    CategoryName = "Jjim",
                    ImageUrl = "https://localhost:7005/CategoryImages/8.png",
                    ImageLocalPath = @"wwwroot\CategoryImages\8.png"
                },
                new Category
                {
                    CategoryId = 9,
                    CategoryName = "Bibimbap",
                    ImageUrl = "https://localhost:7005/CategoryImages/9.png",
                    ImageLocalPath = @"wwwroot\CategoryImages\9.png"
                },
                new Category
                {
                    CategoryId = 10,
                    CategoryName = "Anju",
                    ImageUrl = "https://localhost:7005/CategoryImages/10.png",
                    ImageLocalPath = @"wwwroot\CategoryImages\10.png"
                }
            );
        }
    }
}
