using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace crud
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Item> Items { get; set; }
    }
}

