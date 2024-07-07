using Microsoft.EntityFrameworkCore;
using ServerBlog.Models;

namespace ServerBlog.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<ServeBlog.Models.Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
