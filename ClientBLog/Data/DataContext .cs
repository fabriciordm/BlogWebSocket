using Microsoft.EntityFrameworkCore;
using ClientBLog.Models;
using ClientBLog.Models;

namespace ClientBLog.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
