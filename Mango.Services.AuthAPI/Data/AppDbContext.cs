using Mango.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mango_Services.AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var hasher = new PasswordHasher<ApplicationUser>();

            // Create Admin User
            var adminUser = new ApplicationUser
            {
                Id = "1", // ID of user
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                FirstName = "Admin",
                LastName = "User",
                PhoneNumber ="0784419070",
                Gender = true,
                Address = "123 Admin St",
                Birthday = new DateTime(1990, 1, 1),
                IsActive = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123*");

            // Create Admin Role
            var adminRole = new IdentityRole
            {
                Id = "101", // ID of role
                Name = "ADMIN",
                NormalizedName = "ADMIN"
            };

            // Assign Admin Role to Admin User
            var adminUserRole = new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            };

            // Create Customer User 1 (Vo Minh Nhut)
            var customerUser1 = new ApplicationUser
            {
                Id = "2", // ID of customer
                UserName = "nhutvmce171686@fpt.edu.vn",
                NormalizedUserName = "NHUTVMCE171686@FPT.EDU.VN",
                Email = "nhutvmce171686@fpt.edu.vn",
                NormalizedEmail = "NHUTVMCE171686@FPT.EDU.VN",
                FirstName = "Vo",
                LastName = "Minh Nhut",
                PhoneNumber ="0784419070",
                Gender = true,
                Address = "456 Customer St",
                Birthday = new DateTime(1995, 5, 15),
                IsActive = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            customerUser1.PasswordHash = hasher.HashPassword(customerUser1, "Customer123*");

            // Create Customer Role
            var customerRole = new IdentityRole
            {
                Id = "102", // ID of role
                Name = "CUSTOMER",
                NormalizedName = "CUSTOMER"
            };

            // Assign Customer Role to Customer User 1
            var customerUserRole1 = new IdentityUserRole<string>
            {
                UserId = customerUser1.Id,
                RoleId = customerRole.Id
            };

            // Create Customer User 2 (Do Dang Khoa)
            var customerUser2 = new ApplicationUser
            {
                Id = "3",
                UserName = "khoaddce170883@fpt.edu.vn",
                NormalizedUserName = "KHOADDCE170883@FPT.EDU.VN",
                Email = "khoaddce170883@fpt.edu.vn",
                NormalizedEmail = "KHOADDCE170883@FPT.EDU.VN",
                FirstName = "Do",
                LastName = "Dang Khoa",
                PhoneNumber ="0784419070",
                Gender = true,
                Address = "789 Customer St",
                Birthday = new DateTime(1996, 6, 20),
                IsActive = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            customerUser2.PasswordHash = hasher.HashPassword(customerUser2, "Customer123*");

            // Assign Customer Role to Customer User 2
            var customerUserRole2 = new IdentityUserRole<string>
            {
                UserId = customerUser2.Id,
                RoleId = customerRole.Id
            };

            // Create Customer User 3 (Ho Minh Nhat)
            var customerUser3 = new ApplicationUser
            {
                Id = "4",
                UserName = "nhathmce170171@fpt.edu.vn",
                NormalizedUserName = "NHATHMCE170171@FPT.EDU.VN",
                Email = "nhathmce170171@fpt.edu.vn",
                NormalizedEmail = "NHATHMCE170171@FPT.EDU.VN",
                FirstName = "Ho",
                LastName = "Minh Nhat",
                PhoneNumber ="0784419070",
                Gender = true,
                Address = "1010 Customer St",
                Birthday = new DateTime(1997, 7, 25),
                IsActive = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            customerUser3.PasswordHash = hasher.HashPassword(customerUser3, "Customer123*");

            // Assign Customer Role to Customer User 3
            var customerUserRole3 = new IdentityUserRole<string>
            {
                UserId = customerUser3.Id,
                RoleId = customerRole.Id
            };

            // Create Customer User 4 (Nguyen Huynh Nhu)
            var customerUser4 = new ApplicationUser
            {
                Id = "5",
                UserName = "nhunhce170053@fpt.edu.vn",
                NormalizedUserName = "NHUNHCE170053@FPT.EDU.VN",
                Email = "nhunhce170053@fpt.edu.vn",
                NormalizedEmail = "NHUNHCE170053@FPT.EDU.VN",
                FirstName = "Nguyen",
                LastName = "Huynh Nhu",
                PhoneNumber ="0784419070",
                Gender = false,
                Address = "1112 Customer St",
                Birthday = new DateTime(1998, 8, 10),
                IsActive = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            customerUser4.PasswordHash = hasher.HashPassword(customerUser4, "Customer123*");

            // Assign Customer Role to Customer User 4
            var customerUserRole4 = new IdentityUserRole<string>
            {
                UserId = customerUser4.Id,
                RoleId = customerRole.Id
            };

            // Create Customer User 5 (Nguyen Tran Trung Thanh)
            var customerUser5 = new ApplicationUser
            {
                Id = "6",
                UserName = "thanhntce170901@fpt.edu.vn",
                NormalizedUserName = "THANHNTCE170901@FPT.EDU.VN",
                Email = "thanhntce170901@fpt.edu.vn",
                NormalizedEmail = "THANHNTCE170901@FPT.EDU.VN",
                FirstName = "Nguyen Tran",
                LastName = "Trung Thanh",
                PhoneNumber ="0784419070",
                Gender = true,
                Address = "1314 Customer St",
                Birthday = new DateTime(1999, 9, 5),
                IsActive = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            customerUser5.PasswordHash = hasher.HashPassword(customerUser5, "Customer123*");

            // Assign Customer Role to Customer User 5
            var customerUserRole5 = new IdentityUserRole<string>
            {
                UserId = customerUser5.Id,
                RoleId = customerRole.Id
            };

            // Add users to the database
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser, customerUser1, customerUser2, customerUser3, customerUser4, customerUser5);
            modelBuilder.Entity<IdentityRole>().HasData(adminRole, customerRole);

            // Add user-role relationships to the database
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(adminUserRole);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(customerUserRole1, customerUserRole2, customerUserRole3, customerUserRole4, customerUserRole5);
        }

    }
}
