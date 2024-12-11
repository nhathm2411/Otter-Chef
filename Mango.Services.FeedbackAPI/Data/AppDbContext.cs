using Mango.Services.FeedbackAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.FeedbackAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
