using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mango.Services.AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "101", null, "ADMIN", "ADMIN" },
                    { "102", null, "CUSTOMER", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "Birthday", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "Gender", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "123 Admin St", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "9e735aa0-0133-4c00-87a2-f19f538aaa9e", "admin@gmail.com", true, "Admin", true, true, "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAELOhAnW/1n43Hl6ZFk6cpDg5BS2NADayHczW3FqUhoYazvatULxjMYgQPyepwzLzPg==", "0784419070", false, "de0f32bf-0e11-4b09-bf18-d0cf6124edf6", false, "admin@gmail.com" },
                    { "2", 0, "456 Customer St", new DateTime(1995, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ca158788-8432-4ac7-b664-5059523c69d6", "nhutvmce171686@fpt.edu.vn", true, "Vo", true, true, "Minh Nhut", false, null, "NHUTVMCE171686@FPT.EDU.VN", "NHUTVMCE171686@FPT.EDU.VN", "AQAAAAIAAYagAAAAEOVh9WrbEZukKdNhBuwGCgIQsIrB5et1a3ngL/K0EiYEPtk0cYda8cxkm58U9kfQCw==", "0784419070", false, "1ea0455c-9722-4340-9b4a-35429e231959", false, "nhutvmce171686@fpt.edu.vn" },
                    { "3", 0, "789 Customer St", new DateTime(1996, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "678dd9d3-1ebc-43e6-9291-6942484caa67", "khoaddce170883@fpt.edu.vn", true, "Do", true, true, "Dang Khoa", false, null, "KHOADDCE170883@FPT.EDU.VN", "KHOADDCE170883@FPT.EDU.VN", "AQAAAAIAAYagAAAAEKnIeGlGn9VHkLqZjGT0d70L8YXQP5lvq47vO8XB0m0XkwMtcGFRK2Qm44yanX+TOg==", "0784419070", false, "76eb13fc-8cd1-4363-94c2-c1893e86ef68", false, "khoaddce170883@fpt.edu.vn" },
                    { "4", 0, "1010 Customer St", new DateTime(1997, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "174afb2a-288d-48e6-8131-917c1c61ae24", "nhathmce170171@fpt.edu.vn", true, "Ho", true, true, "Minh Nhat", false, null, "NHATHMCE170171@FPT.EDU.VN", "NHATHMCE170171@FPT.EDU.VN", "AQAAAAIAAYagAAAAEJyVE9byz/Z2m4SSzzJxUCAkITSSsxmXv5Py+RB0q53w1fvnU9O8BNya7H93oOMtDA==", "0784419070", false, "2a885fd9-69c8-49d3-aa69-3d4a490bae49", false, "nhathmce170171@fpt.edu.vn" },
                    { "5", 0, "1112 Customer St", new DateTime(1998, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "11df937b-b074-44a3-8a4f-16e0d56406ff", "nhunhce170053@fpt.edu.vn", true, "Nguyen", false, true, "Huynh Nhu", false, null, "NHUNHCE170053@FPT.EDU.VN", "NHUNHCE170053@FPT.EDU.VN", "AQAAAAIAAYagAAAAEC+RVP3TUcqSqIDg/vqEQGPUGW7lEpuCBrAsynH7ZS4aHar0qW/SZPQC4rWQzvGqTA==", "0784419070", false, "22677e70-b80c-4315-ad67-7764d89a54fb", false, "nhunhce170053@fpt.edu.vn" },
                    { "6", 0, "1314 Customer St", new DateTime(1999, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "a4a45ca7-9e6d-4321-896b-8f40faebb864", "thanhntce170901@fpt.edu.vn", true, "Nguyen Tran", true, true, "Trung Thanh", false, null, "THANHNTCE170901@FPT.EDU.VN", "THANHNTCE170901@FPT.EDU.VN", "AQAAAAIAAYagAAAAEC2MUcWTK81ccF53x/StlgBMJTMmtk36lRuJkFlCEkZpJMa9mqVxwJSDcdvNxOMlEA==", "0784419070", false, "7444a91f-fde7-45cf-a1a8-0d71c3157169", false, "thanhntce170901@fpt.edu.vn" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "101", "1" },
                    { "102", "2" },
                    { "102", "3" },
                    { "102", "4" },
                    { "102", "5" },
                    { "102", "6" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
