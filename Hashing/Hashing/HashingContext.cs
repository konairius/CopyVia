using Microsoft.EntityFrameworkCore;

namespace Hashing
{
    public class HashingContext : DbContext
    {
        public HashingContext() {
        }

        public HashingContext(DbContextOptions options): base(options) {
        }

        public DbSet<HashInfo> HashInfos { get; set; }
    }
}
