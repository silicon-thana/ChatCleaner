using Microsoft.EntityFrameworkCore;
using Data.Data.Entities;

namespace ChatCleaner.Data.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
