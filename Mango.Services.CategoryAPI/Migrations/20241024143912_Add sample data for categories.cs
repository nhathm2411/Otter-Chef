using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mango.Services.CategoryAPI.Migrations
{
    /// <inheritdoc />
    public partial class Addsampledataforcategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageLocalPath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "ImageLocalPath", "ImageUrl" },
                values: new object[,]
                {
                    { 1, "Kimchi", "wwwroot\\CategoryImages\\1.png", "https://localhost:7005/CategoryImages/1.png" },
                    { 2, "Jjigae", "wwwroot\\CategoryImages\\2.png", "https://localhost:7005/CategoryImages/2.png" },
                    { 3, "Banchan", "wwwroot\\CategoryImages\\3.png", "https://localhost:7005/CategoryImages/3.png" },
                    { 4, "Bulgogi and Gui", "wwwroot\\CategoryImages\\4.png", "https://localhost:7005/CategoryImages/4.png" },
                    { 5, "Tteok", "wwwroot\\CategoryImages\\5.png", "https://localhost:7005/CategoryImages/5.png" },
                    { 6, "Jeon", "wwwroot\\CategoryImages\\6.png", "https://localhost:7005/CategoryImages/6.png" },
                    { 7, "Guksu", "wwwroot\\CategoryImages\\7.png", "https://localhost:7005/CategoryImages/7.png" },
                    { 8, "Jjim", "wwwroot\\CategoryImages\\8.png", "https://localhost:7005/CategoryImages/8.png" },
                    { 9, "Bibimbap", "wwwroot\\CategoryImages\\9.png", "https://localhost:7005/CategoryImages/9.png" },
                    { 10, "Anju", "wwwroot\\CategoryImages\\10.png", "https://localhost:7005/CategoryImages/10.png" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
