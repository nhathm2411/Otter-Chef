using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mango.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLocalPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Description", "ImageLocalPath", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Traditional napa cabbage kimchi, made with salted cabbage, chili flakes, garlic, and fermented seafood.", "wwwroot/ProductImages/1.png", "https://localhost:7000/ProductImages/1.png", "Baechu Kimchi", 5.0 },
                    { 2, 1, "Cubed radish kimchi, crunchy and spicy, usually served with soups and stews.", "wwwroot/ProductImages/2.png", "https://localhost:7000/ProductImages/2.png", "Kkakdugi", 5.0 },
                    { 3, 1, "White kimchi, a non-spicy version made without chili pepper, offering a mild and refreshing flavor.", "wwwroot/ProductImages/3.png", "https://localhost:7000/ProductImages/3.png", "Baek Kimchi", 6.0 },
                    { 4, 2, "Spicy kimchi stew, typically made with aged kimchi, pork, tofu, and vegetables, simmered in a rich broth.", "wwwroot/ProductImages/4.png", "https://localhost:7000/ProductImages/4.png", "Kimchi Jjigae", 8.0 },
                    { 5, 2, "Fermented soybean paste stew with vegetables, tofu, and sometimes clams or anchovies, offering a savory and umami-rich flavor.", "wwwroot/ProductImages/5.png", "https://localhost:7000/ProductImages/5.png", "Doenjang Jjigae", 7.0 },
                    { 6, 2, "Soft tofu stew with seafood or pork, featuring a spicy broth and silky soft tofu.", "wwwroot/ProductImages/6.png", "https://localhost:7000/ProductImages/6.png", "Sundubu Jjigae", 8.0 },
                    { 7, 3, "Stir-fried glass noodles with vegetables, soy sauce, and sesame oil, often served as a side dish or appetizer.", "wwwroot/ProductImages/7.png", "https://localhost:7000/ProductImages/7.png", "Japchae", 7.0 },
                    { 8, 3, "Seasoned bean sprout salad, lightly blanched and tossed with sesame oil, garlic, and salt.", "wwwroot/ProductImages/8.png", "https://localhost:7000/ProductImages/8.png", "Kongnamul Muchim", 3.0 },
                    { 9, 3, "Stir-fried kimchi, often with pork or tuna, giving a smoky and intense flavor.", "wwwroot/ProductImages/9.png", "https://localhost:7000/ProductImages/9.png", "Kimchi Bokkeum", 5.0 },
                    { 10, 4, "Thinly sliced marinated beef, grilled or stir-fried, served with rice and various side dishes.", "wwwroot/ProductImages/10.png", "https://localhost:7000/ProductImages/10.png", "Bulgogi", 12.0 },
                    { 11, 4, "Grilled pork belly, typically enjoyed with lettuce wraps, garlic, and ssamjang (a savory dipping sauce).", "wwwroot/ProductImages/11.png", "https://localhost:7000/ProductImages/11.png", "Samgyeopsal", 10.0 },
                    { 12, 4, "Marinated beef short ribs, either grilled (Galbi Gui) or braised (Galbi Jjim), known for their sweet and savory flavor.", "wwwroot/ProductImages/12.png", "https://localhost:7000/ProductImages/12.png", "Galbi", 15.0 },
                    { 13, 5, "Spicy stir-fried rice cakes in a gochujang-based sauce, often served with fish cakes and boiled eggs.", "wwwroot/ProductImages/13.png", "https://localhost:7000/ProductImages/13.png", "Tteokbokki", 7.0 },
                    { 14, 5, "Royal court-style Tteokbokki, featuring a mild soy-based sauce with beef and vegetables.", "wwwroot/ProductImages/14.png", "https://localhost:7000/ProductImages/14.png", "Gungjung Tteokbokki", 8.0 },
                    { 15, 5, "Soft rice cakes coated with roasted soybean powder, offering a chewy texture and nutty flavor.", "wwwroot/ProductImages/15.png", "https://localhost:7000/ProductImages/15.png", "Injeolmi", 6.0 },
                    { 16, 6, "Savory pancake made with green onions and mixed seafood, served with a soy-vinegar dipping sauce.", "wwwroot/ProductImages/16.png", "https://localhost:7000/ProductImages/16.png", "Haemul Pajeon", 12.0 },
                    { 17, 6, "Kimchi pancake, made with fermented kimchi, flour, and sometimes added pork or seafood.", "wwwroot/ProductImages/17.png", "https://localhost:7000/ProductImages/17.png", "Kimchi Jeon", 7.0 },
                    { 18, 6, "Mung bean pancake with ground mung beans, vegetables, and pork, fried until crispy on the outside.", "wwwroot/ProductImages/18.png", "https://localhost:7000/ProductImages/18.png", "Bindaetteok", 9.0 },
                    { 19, 7, "Cold buckwheat noodles, typically served in a chilled broth (Mul Naengmyeon) or with a spicy sauce (Bibim Naengmyeon).", "wwwroot/ProductImages/19.png", "https://localhost:7000/ProductImages/19.png", "Naengmyeon", 11.0 },
                    { 20, 7, "Noodles in black bean sauce with diced pork and vegetables, known for its rich and slightly sweet flavor.", "wwwroot/ProductImages/20.png", "https://localhost:7000/ProductImages/20.png", "Jajangmyeon", 6.0 },
                    { 21, 7, "Hand-cut wheat noodles served in a clear broth, usually made with chicken or seafood.", "wwwroot/ProductImages/21.png", "https://localhost:7000/ProductImages/21.png", "Kalguksu", 14.0 },
                    { 22, 8, "Korean rice rolls filled with vegetables, fish, or meat, wrapped in seaweed, perfect for a light meal or snack.", "wwwroot/ProductImages/22.png", "https://localhost:7000/ProductImages/22.png", "Kimbap", 5.0 },
                    { 23, 8, "Mixed rice with assorted vegetables, meat, and a fried egg, topped with gochujang (spicy red pepper paste).", "wwwroot/ProductImages/23.png", "https://localhost:7000/ProductImages/23.png", "Bibimbap", 10.0 },
                    { 24, 8, "Korean blood sausage made with pig's blood, rice, and noodles, often served with a spicy dipping sauce.", "wwwroot/ProductImages/24.png", "https://localhost:7000/ProductImages/24.png", "Sundae", 8.0 },
                    { 25, 9, "Sweet Korean pancakes filled with brown sugar, honey, chopped peanuts, and cinnamon, usually served warm.", "wwwroot/ProductImages/25.png", "https://localhost:7000/ProductImages/25.png", "Hoddeok", 4.0 },
                    { 26, 9, "Fish-shaped pastry filled with sweet red bean paste, popular as a street food snack.", "wwwroot/ProductImages/26.png", "https://localhost:7000/ProductImages/26.png", "Bungeoppang", 3.0 },
                    { 27, 9, "Korean rice cakes made from glutinous rice flour, often enjoyed during celebrations.", "wwwroot/ProductImages/27.png", "https://localhost:7000/ProductImages/27.png", "Tteok", 5.0 },
                    { 28, 10, "Deep-fried dough pastries, often coated in cinnamon sugar, typically served with chocolate dipping sauce.", "wwwroot/ProductImages/28.png", "https://localhost:7000/ProductImages/28.png", "Churros", 6.0 },
                    { 29, 10, "Sweet syrup-filled pancakes, usually served hot and often eaten during the winter.", "wwwroot/ProductImages/29.png", "https://localhost:7000/ProductImages/29.png", "Hotteok", 4.0 },
                    { 30, 10, "Shaved ice dessert topped with sweetened red beans, fruit, and condensed milk, popular in the summer.", "wwwroot/ProductImages/30.png", "https://localhost:7000/ProductImages/30.png", "Patbingsu", 8.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
