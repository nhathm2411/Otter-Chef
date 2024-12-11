using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Baechu Kimchi",
                    Price = 5,
                    Description = "Traditional napa cabbage kimchi, made with salted cabbage, chili flakes, garlic, and fermented seafood.",
                    CategoryId = 1,
                    ImageUrl = "https://localhost:7000/ProductImages/1.png",
                    ImageLocalPath = "wwwroot/ProductImages/1.png"
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Kkakdugi",
                    Price = 5,
                    Description = "Cubed radish kimchi, crunchy and spicy, usually served with soups and stews.",
                    CategoryId = 1,
                    ImageUrl = "https://localhost:7000/ProductImages/2.png",
                    ImageLocalPath = "wwwroot/ProductImages/2.png"
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Baek Kimchi",
                    Price = 6,
                    Description = "White kimchi, a non-spicy version made without chili pepper, offering a mild and refreshing flavor.",
                    CategoryId = 1,
                    ImageUrl = "https://localhost:7000/ProductImages/3.png",
                    ImageLocalPath = "wwwroot/ProductImages/3.png"
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Kimchi Jjigae",
                    Price = 8,
                    Description = "Spicy kimchi stew, typically made with aged kimchi, pork, tofu, and vegetables, simmered in a rich broth.",
                    CategoryId = 2,
                    ImageUrl = "https://localhost:7000/ProductImages/4.png",
                    ImageLocalPath = "wwwroot/ProductImages/4.png"
                },
                new Product
                {
                    ProductId = 5,
                    Name = "Doenjang Jjigae",
                    Price = 7,
                    Description = "Fermented soybean paste stew with vegetables, tofu, and sometimes clams or anchovies, offering a savory and umami-rich flavor.",
                    CategoryId = 2,
                    ImageUrl = "https://localhost:7000/ProductImages/5.png",
                    ImageLocalPath = "wwwroot/ProductImages/5.png"
                },
                new Product
                {
                    ProductId = 6,
                    Name = "Sundubu Jjigae",
                    Price = 8,
                    Description = "Soft tofu stew with seafood or pork, featuring a spicy broth and silky soft tofu.",
                    CategoryId = 2,
                    ImageUrl = "https://localhost:7000/ProductImages/6.png",
                    ImageLocalPath = "wwwroot/ProductImages/6.png"
                },
                new Product
                {
                    ProductId = 7,
                    Name = "Japchae",
                    Price = 7,
                    Description = "Stir-fried glass noodles with vegetables, soy sauce, and sesame oil, often served as a side dish or appetizer.",
                    CategoryId = 3,
                    ImageUrl = "https://localhost:7000/ProductImages/7.png",
                    ImageLocalPath = "wwwroot/ProductImages/7.png"
                },
                new Product
                {
                    ProductId = 8,
                    Name = "Kongnamul Muchim",
                    Price = 3,
                    Description = "Seasoned bean sprout salad, lightly blanched and tossed with sesame oil, garlic, and salt.",
                    CategoryId = 3,
                    ImageUrl = "https://localhost:7000/ProductImages/8.png",
                    ImageLocalPath = "wwwroot/ProductImages/8.png"
                },
                new Product
                {
                    ProductId = 9,
                    Name = "Kimchi Bokkeum",
                    Price = 5,
                    Description = "Stir-fried kimchi, often with pork or tuna, giving a smoky and intense flavor.",
                    CategoryId = 3,
                    ImageUrl = "https://localhost:7000/ProductImages/9.png",
                    ImageLocalPath = "wwwroot/ProductImages/9.png"
                },
                new Product
                {
                    ProductId = 10,
                    Name = "Bulgogi",
                    Price = 12,
                    Description = "Thinly sliced marinated beef, grilled or stir-fried, served with rice and various side dishes.",
                    CategoryId = 4,
                    ImageUrl = "https://localhost:7000/ProductImages/10.png",
                    ImageLocalPath = "wwwroot/ProductImages/10.png"
                },
                new Product
                {
                    ProductId = 11,
                    Name = "Samgyeopsal",
                    Price = 10,
                    Description = "Grilled pork belly, typically enjoyed with lettuce wraps, garlic, and ssamjang (a savory dipping sauce).",
                    CategoryId = 4,
                    ImageUrl = "https://localhost:7000/ProductImages/11.png",
                    ImageLocalPath = "wwwroot/ProductImages/11.png"
                },
                new Product
                {
                    ProductId = 12,
                    Name = "Galbi",
                    Price = 15,
                    Description = "Marinated beef short ribs, either grilled (Galbi Gui) or braised (Galbi Jjim), known for their sweet and savory flavor.",
                    CategoryId = 4,
                    ImageUrl = "https://localhost:7000/ProductImages/12.png",
                    ImageLocalPath = "wwwroot/ProductImages/12.png"
                },
                new Product
                {
                    ProductId = 13,
                    Name = "Tteokbokki",
                    Price = 7,
                    Description = "Spicy stir-fried rice cakes in a gochujang-based sauce, often served with fish cakes and boiled eggs.",
                    CategoryId = 5,
                    ImageUrl = "https://localhost:7000/ProductImages/13.png",
                    ImageLocalPath = "wwwroot/ProductImages/13.png"
                },
                new Product
                {
                    ProductId = 14,
                    Name = "Gungjung Tteokbokki",
                    Price = 8,
                    Description = "Royal court-style Tteokbokki, featuring a mild soy-based sauce with beef and vegetables.",
                    CategoryId = 5,
                    ImageUrl = "https://localhost:7000/ProductImages/14.png",
                    ImageLocalPath = "wwwroot/ProductImages/14.png"
                },
                new Product
                {
                    ProductId = 15,
                    Name = "Injeolmi",
                    Price = 6,
                    Description = "Soft rice cakes coated with roasted soybean powder, offering a chewy texture and nutty flavor.",
                    CategoryId = 5,
                    ImageUrl = "https://localhost:7000/ProductImages/15.png",
                    ImageLocalPath = "wwwroot/ProductImages/15.png"
                },
                new Product
                {
                    ProductId = 16,
                    Name = "Haemul Pajeon",
                    Price = 12,
                    Description = "Savory pancake made with green onions and mixed seafood, served with a soy-vinegar dipping sauce.",
                    CategoryId = 6,
                    ImageUrl = "https://localhost:7000/ProductImages/16.png",
                    ImageLocalPath = "wwwroot/ProductImages/16.png"
                },
                new Product
                {
                    ProductId = 17,
                    Name = "Kimchi Jeon",
                    Price = 7,
                    Description = "Kimchi pancake, made with fermented kimchi, flour, and sometimes added pork or seafood.",
                    CategoryId = 6,
                    ImageUrl = "https://localhost:7000/ProductImages/17.png",
                    ImageLocalPath = "wwwroot/ProductImages/17.png"
                },
                new Product
                {
                    ProductId = 18,
                    Name = "Bindaetteok",
                    Price = 9,
                    Description = "Mung bean pancake with ground mung beans, vegetables, and pork, fried until crispy on the outside.",
                    CategoryId = 6,
                    ImageUrl = "https://localhost:7000/ProductImages/18.png",
                    ImageLocalPath = "wwwroot/ProductImages/18.png"
                },
                new Product
                {
                    ProductId = 19,
                    Name = "Naengmyeon",
                    Price = 11,
                    Description = "Cold buckwheat noodles, typically served in a chilled broth (Mul Naengmyeon) or with a spicy sauce (Bibim Naengmyeon).",
                    CategoryId = 7,
                    ImageUrl = "https://localhost:7000/ProductImages/19.png",
                    ImageLocalPath = "wwwroot/ProductImages/19.png"
                },
                new Product
                {
                    ProductId = 20,
                    Name = "Jajangmyeon",
                    Price = 6,
                    Description = "Noodles in black bean sauce with diced pork and vegetables, known for its rich and slightly sweet flavor.",
                    CategoryId = 7,
                    ImageUrl = "https://localhost:7000/ProductImages/20.png",
                    ImageLocalPath = "wwwroot/ProductImages/20.png"
                },
                new Product
                {
                    ProductId = 21,
                    Name = "Kalguksu",
                    Price = 14,
                    Description = "Hand-cut wheat noodles served in a clear broth, usually made with chicken or seafood.",
                    CategoryId = 7,
                    ImageUrl = "https://localhost:7000/ProductImages/21.png",
                    ImageLocalPath = "wwwroot/ProductImages/21.png",
                },
                new Product
                {
                    ProductId = 22,
                    Name = "Kimbap",
                    Price = 5,
                    Description = "Korean rice rolls filled with vegetables, fish, or meat, wrapped in seaweed, perfect for a light meal or snack.",
                    CategoryId = 8,
                    ImageUrl = "https://localhost:7000/ProductImages/22.png",
                    ImageLocalPath = "wwwroot/ProductImages/22.png"
                },
                new Product
                {
                    ProductId = 23,
                    Name = "Bibimbap",
                    Price = 10,
                    Description = "Mixed rice with assorted vegetables, meat, and a fried egg, topped with gochujang (spicy red pepper paste).",
                    CategoryId = 8,
                    ImageUrl = "https://localhost:7000/ProductImages/23.png",
                    ImageLocalPath = "wwwroot/ProductImages/23.png"
                },
                new Product
                {
                    ProductId = 24,
                    Name = "Sundae",
                    Price = 8,
                    Description = "Korean blood sausage made with pig's blood, rice, and noodles, often served with a spicy dipping sauce.",
                    CategoryId = 8,
                    ImageUrl = "https://localhost:7000/ProductImages/24.png",
                    ImageLocalPath = "wwwroot/ProductImages/24.png"
                },
                 new Product
                 {
                     ProductId = 25,
                     Name = "Hoddeok",
                     Price = 4,
                     Description = "Sweet Korean pancakes filled with brown sugar, honey, chopped peanuts, and cinnamon, usually served warm.",
                     CategoryId = 9,
                     ImageUrl = "https://localhost:7000/ProductImages/25.png",
                     ImageLocalPath = "wwwroot/ProductImages/25.png"
                 },
                 new Product
                 {
                     ProductId = 26,
                     Name = "Bungeoppang",
                     Price = 3,
                     Description = "Fish-shaped pastry filled with sweet red bean paste, popular as a street food snack.",
                     CategoryId = 9,
                     ImageUrl = "https://localhost:7000/ProductImages/26.png",
                     ImageLocalPath = "wwwroot/ProductImages/26.png"
                 },
                new Product
                {
                    ProductId = 27,
                    Name = "Tteok",
                    Price = 5,
                    Description = "Korean rice cakes made from glutinous rice flour, often enjoyed during celebrations.",
                    CategoryId = 9,
                    ImageUrl = "https://localhost:7000/ProductImages/27.png",
                    ImageLocalPath = "wwwroot/ProductImages/27.png"
                },
                 new Product
                 {
                     ProductId = 28,
                     Name = "Churros",
                     Price = 6,
                     Description = "Deep-fried dough pastries, often coated in cinnamon sugar, typically served with chocolate dipping sauce.",
                     CategoryId = 10,
                     ImageUrl = "https://localhost:7000/ProductImages/28.png",
                     ImageLocalPath = "wwwroot/ProductImages/28.png"
                 },
                 new Product
                 {
                     ProductId = 29,
                     Name = "Hotteok",
                     Price = 4,
                     Description = "Sweet syrup-filled pancakes, usually served hot and often eaten during the winter.",
                     CategoryId = 10,
                     ImageUrl = "https://localhost:7000/ProductImages/29.png",
                     ImageLocalPath = "wwwroot/ProductImages/29.png"
                 },
                 new Product
                 {
                     ProductId = 30,
                     Name = "Patbingsu",
                     Price = 8,
                     Description = "Shaved ice dessert topped with sweetened red beans, fruit, and condensed milk, popular in the summer.",
                     CategoryId = 10,
                     ImageUrl = "https://localhost:7000/ProductImages/30.png",
                     ImageLocalPath = "wwwroot/ProductImages/30.png"
                 });
        }
    }
}
