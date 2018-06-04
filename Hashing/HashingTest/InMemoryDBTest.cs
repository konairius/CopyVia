using System;
using System.IO;
using System.Linq;
using Hashing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HashingTest
{
    public class CleanDbTest
    {
        [Fact]
        public void SetupDatabase()
        {
            using (CleanDbInfo dbInfo = new CleanDbInfo())
            using (HashingContext db = new HashingContext(dbInfo.Options))
            {
                db.Database.Migrate();
                Assert.Equal(0, db.HashInfos.Count());
            }
        }

        [Fact]
        public void AddHashToDatabase()
        {
            using (CleanDbInfo dbInfo = new CleanDbInfo())
            using (HashingContext db = new HashingContext(dbInfo.Options))
            {
                db.Database.Migrate();
                HashInfo info = HashInfo.FromFile("D:\\router.tauron.zip");
                db.HashInfos.Add(info);
                db.SaveChanges();
            }
        }
    }

    public class CleanDbInfo : IDisposable
    {
        public CleanDbInfo() { TmpPath = Path.GetTempFileName(); }

        private String TmpPath { get; }
        public DbContextOptions Options
        {
            get
            {
                SqliteConnectionStringBuilder conntectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = TmpPath };
                DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
                builder.UseSqlite(conntectionStringBuilder.ConnectionString);
                return builder.Options;
            }
        }


        public void Dispose()
        {
            File.Delete(TmpPath);
        }
    }
}
